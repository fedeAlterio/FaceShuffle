using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json;
using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.PendingJobs.ExecutePendingJob.PendingJobActions.Abstractions;
using FaceShuffle.Application.Extensions;
using FaceShuffle.Models.PendingJobs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace FaceShuffle.Application.Actions.PendingJobs.ExecutePendingJob;
public class ExecutePendingJobHandler : IRequestHandler<ExecutePendingJobRequest, ExecutePendingJobResponse>
{
    record PendingJobRequestInfo(Type RequestType,
        Type PayloadType,
        PropertyInfo PayloadPropertyInfo,
        Policy Policy);

    private static readonly ImmutableDictionary<PendingJobType, PendingJobRequestInfo> HandlersByPendingJobType = GetHandlersByPendingJobType();
    private readonly IServiceProvider _serviceProvider;
    private readonly IAppDbContext _appDbContext;
    private readonly ILogger<ExecutePendingJobHandler> _logger;

    public ExecutePendingJobHandler(IServiceProvider serviceProvider,
        IAppDbContext appDbContext,
        ILogger<ExecutePendingJobHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _appDbContext = appDbContext;
        _logger = logger;
    }

    public async Task<ExecutePendingJobResponse> Handle(ExecutePendingJobRequest request, CancellationToken cancellationToken)
    {
        var handlerRequestInfo = HandlersByPendingJobType[request.PendingJob.Type];
        var payload = JsonSerializer.Deserialize(request.PendingJob.Payload, handlerRequestInfo.PayloadType);
        var handlerRequest = (IPendingJobRequest)Activator.CreateInstance(handlerRequestInfo.RequestType)!;
        handlerRequestInfo.PayloadPropertyInfo.SetValue(handlerRequest, payload);

        var mediator = _serviceProvider.GetRequiredService<IMediator>();

        var pendingJob = request.PendingJob;
        _logger.LogInformation("Executing pending job {Job}", pendingJob);

        try
        {
            await handlerRequestInfo.Policy.Execute(async () =>
            {
                await mediator.Send(handlerRequest, cancellationToken);
                _appDbContext.PendingJobs.DbSet.Remove(request.PendingJob);
                await _appDbContext.SaveChangesAsync(cancellationToken);
            });
        }
        catch (Exception e)
        {
            pendingJob.PendingJobStatus = PendingJobStatus.Failed;
            await _appDbContext.SaveChangesAsync(cancellationToken);
            _logger.LogError(e, "Failed to execute pending job {Job}", request.PendingJob);
            throw;
        }

        return new();
    }

    private static ImmutableDictionary<PendingJobType, PendingJobRequestInfo> GetHandlersByPendingJobType()
    {
        object? GetStaticPropertyValue(Type requestType, string propertyName)
        {
            return requestType.GetProperty(propertyName)!.GetMethod!.Invoke(null, Array.Empty<object>());
        }

        return (from requestType in typeof(ExecutePendingJobHandler).Assembly.GetAllTypesImplementingOpenGenericType(typeof(IPendingJobRequest<>))
                let pendingJobType = (PendingJobType)GetStaticPropertyValue(requestType, nameof(IPendingJobRequest<Unit>.PendingJobType))!
                let payloadProperty = requestType.GetProperty(nameof(IPendingJobRequest<Unit>.Payload))!
                let payloadType = payloadProperty.PropertyType!
                let policy = (Policy) GetStaticPropertyValue(requestType, nameof(IPendingJobRequest<Unit>.PendingJobPolicy))!
                select new { pendingJobType, requestType, payloadType, payloadProperty, policy })
            .ToImmutableDictionary(x => x.pendingJobType, x => new PendingJobRequestInfo(x.requestType,
                x.payloadType,
                x.payloadProperty!,
                x.policy));
    }

}
