using FaceShuffle.Application.Actions.PendingJobs.BackgroundServices;
using FaceShuffle.Models.PendingJobs.Events;
using MediatR;

namespace FaceShuffle.Application.Actions.PendingJobs.EventHandlers;
public class PendingJobAddedEventHandler : INotificationHandler<PendingJobAddedEvent>
{
    private readonly ExecutePendingJobsBackgroundService _pendingJobExecutor;

    public PendingJobAddedEventHandler(ExecutePendingJobsBackgroundService pendingJobExecutor)
    {
        _pendingJobExecutor = pendingJobExecutor;
    }

    public Task Handle(PendingJobAddedEvent notification, CancellationToken cancellationToken)
    {
        _pendingJobExecutor.SchedulePendingJobs();
        return Task.CompletedTask;
    }
}
