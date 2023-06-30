using FaceShuffle.Models.PendingJobs;

namespace FaceShuffle.Application.Actions.PendingJobs.ExecutePendingJob.PendingJobActions.Abstractions;
public interface IPendingJobRequest<T>
{
    static abstract PendingJobType PendingJobType { get; }
    T Payload { get; init; }
}
