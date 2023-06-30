using FaceShuffle.Models.PendingJobs;
using Polly;

namespace FaceShuffle.Application.Actions.PendingJobs.ExecutePendingJob.PendingJobActions.Abstractions;

public interface IPendingJobRequest
{
    static abstract Policy PendingJobPolicy { get; }
}

public interface IPendingJobRequest<T> : IPendingJobRequest
{
    static abstract PendingJobType PendingJobType { get; }
    T Payload { get; init; }
}
