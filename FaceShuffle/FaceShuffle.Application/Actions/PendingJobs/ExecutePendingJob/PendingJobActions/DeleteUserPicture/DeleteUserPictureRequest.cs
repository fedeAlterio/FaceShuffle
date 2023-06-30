using FaceShuffle.Application.Actions.PendingJobs.ExecutePendingJob.PendingJobActions.Abstractions;
using FaceShuffle.Models.PendingJobs;
using MediatR;

namespace FaceShuffle.Application.Actions.PendingJobs.ExecutePendingJob.PendingJobActions.DeleteUserPicture;
public class DeleteUserPictureRequest : IPendingJobRequest<DeleteUserPicturesPendingJob>, IRequest<DeleteUserPictureResponse>
{
    public static PendingJobType PendingJobType => PendingJobType.DeleteUserPicture;
    public required DeleteUserPicturesPendingJob Payload { get; init; }
}
