using FaceShuffle.Application.Actions.PendingJobs.ExecutePendingJob.PendingJobActions.Abstractions;
using FaceShuffle.Models.PendingJobs;
using MediatR;
using Polly;

namespace FaceShuffle.Application.Actions.PendingJobs.ExecutePendingJob.PendingJobActions.DeleteUserPicture;
public class DeleteUserPictureRequest : IPendingJobRequest<DeleteUserPicturesPendingJob>, 
    IRequest<DeleteUserPictureResponse>
{
    public static PendingJobType PendingJobType => PendingJobType.DeleteUserPicture;
    public static Policy PendingJobPolicy => PendingJobPolicies.DefaultPolicy;
    public required DeleteUserPicturesPendingJob Payload { get; init; }
}
