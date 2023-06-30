using System.Text.Json;
using FaceShuffle.Models.PendingJobs;

namespace FaceShuffle.Application.Actions.PendingJobs.PendingJobEnqueuers;
public static class DeleteUserPicturesEnqueuer
{
    public static async Task EnqueueNewDeleteUserPicturesJob(this IPendingJobService @this, DeleteUserPicturesPendingJob pendingJobPayload, CancellationToken cancellationToken)
    {
        var pendingJob = new PendingJob
        {
            Type = PendingJobType.DeleteUserPicture,
            Payload = JsonSerializer.Serialize(pendingJobPayload),
        };

        await @this.EnqueueNewJob(pendingJob, cancellationToken);
    }
}
