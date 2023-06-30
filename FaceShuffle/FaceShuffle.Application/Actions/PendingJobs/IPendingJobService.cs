using FaceShuffle.Models.PendingJobs;

namespace FaceShuffle.Application.Actions.PendingJobs;
public interface IPendingJobService
{
    Task EnqueueNewJob(PendingJob job, CancellationToken cancellationToken);
}
