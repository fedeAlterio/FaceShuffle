using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.PendingJobs.ExecuteAllPendingJobs;
using FaceShuffle.Models.Extensions;
using FaceShuffle.Models.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FaceShuffle.Application.Actions.PendingJobs.BackgroundServices;
public class ExecutePendingJobsBackgroundService : IBackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ExecutePendingJobsBackgroundService> _logger;
    private AsyncTimer? _timer;

    public ExecutePendingJobsBackgroundService(IServiceProvider serviceProvider,
        ILogger<ExecutePendingJobsBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var timer = new AsyncTimer(TimeSpan.FromSeconds(1000), TimeSpan.Zero, ExecuteAllPendingJobs);
        timer.OnException += Timer_OnException;
        _timer = timer;

        timer.Start();
        await cancellationToken
            .WaitUntilCancellationAsync()
            .Finally(_ => timer.StopAndCancelCurrentAsync());
    }

    private void Timer_OnException(object? sender, Exception e)
    {
        _logger.LogError(e, $"Error on {typeof(ExecutePendingJobsBackgroundService)}");
    }

    public void SchedulePendingJobs() => _timer?.ScheduleAndWaitExtraTickAsync().FireAndForget();

    async Task ExecuteAllPendingJobs(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var sender = scope.ServiceProvider.GetRequiredService<IRequestSender<ExecuteAllPendingJobsRequest, ExecuteAllPendingJobsResponse>>();
        await sender.Send(new(), cancellationToken);
    }
}
