using Polly;

namespace FaceShuffle.Application.Actions.PendingJobs.ExecutePendingJob.PendingJobActions;
public static class PendingJobPolicies
{
    public static readonly Policy DefaultPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetry(new[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(3) });
}
