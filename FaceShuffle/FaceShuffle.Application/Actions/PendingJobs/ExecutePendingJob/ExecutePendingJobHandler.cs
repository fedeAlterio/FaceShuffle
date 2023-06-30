using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json;
using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.PendingJobs.ExecutePendingJob.PendingJobActions.Abstractions;
using FaceShuffle.Application.Extensions;
using FaceShuffle.Models.PendingJobs;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace FaceShuffle.Application.Actions.PendingJobs.ExecutePendingJob;
public class ExecutePendingJobHandler : IRequestHandler<ExecutePendingJobRequest, ExecutePendingJobResponse>
{
    record PendingJobRequestInfo(Type RequestType, Type PayloadType, PropertyInfo PayloadPropertyInfo);
    private static readonly ImmutableDictionary<PendingJobType, PendingJobRequestInfo> HandlersByPendingJobType = GetHandlersByPendingJobType();
    private static Policy _pendingJobHandlerRetryPolicy = GetExecutePendingJobPolicy();
    private readonly IServiceProvider _serviceProvider;
    private readonly IAppDbContext _appDbContext;

    public ExecutePendingJobHandler(IServiceProvider serviceProvider, IAppDbContext appDbContext)
    {
        _serviceProvider = serviceProvider;
        _appDbContext = appDbContext;
    }

    public async Task<ExecutePendingJobResponse> Handle(ExecutePendingJobRequest request, CancellationToken cancellationToken)
    {
        var handlerRequestInfo = HandlersByPendingJobType[request.PendingJob.Type];
        var payload = JsonSerializer.Deserialize(request.PendingJob.Payload, handlerRequestInfo.PayloadType);
        var handlerRequest = Activator.CreateInstance(handlerRequestInfo.RequestType)!;
        handlerRequestInfo.PayloadPropertyInfo.SetValue(handlerRequest, payload);

        var mediator = _serviceProvider.GetRequiredService<IMediator>();

        await _pendingJobHandlerRetryPolicy.Execute(async () =>
        {
            await mediator.Send(handlerRequest, cancellationToken);
            _appDbContext.PendingJobs.DbSet.Remove(request.PendingJob);
            await _appDbContext.SaveChangesAsync(cancellationToken);
        });

        return new();
    }

    static Policy GetExecutePendingJobPolicy()
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetry(new[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(3) });
    }

    private static ImmutableDictionary<PendingJobType, PendingJobRequestInfo> GetHandlersByPendingJobType()
    {
        return (from requestType in typeof(ExecutePendingJobHandler).Assembly.GetAllTypesImplementingOpenGenericType(typeof(IPendingJobRequest<>))
                from member in requestType.GetProperties()
                where member.Name == nameof(IPendingJobRequest<Unit>.PendingJobType)
                let pendingJobType = (PendingJobType)member.GetMethod!.Invoke(null, Array.Empty<object>())!
                let payloadProperty = requestType.GetProperty(nameof(IPendingJobRequest<Unit>.Payload))!
                let payloadType = payloadProperty.PropertyType!
                select new { pendingJobType, requestType, payloadType, payloadProperty})
            .ToImmutableDictionary(x => x.pendingJobType, x => new PendingJobRequestInfo(x.requestType, x.payloadType, x.payloadProperty!));
    }

}
