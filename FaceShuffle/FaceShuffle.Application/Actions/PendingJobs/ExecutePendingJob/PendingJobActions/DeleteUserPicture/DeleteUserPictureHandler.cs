using FaceShuffle.Application.Repositories;
using MediatR;

namespace FaceShuffle.Application.Actions.PendingJobs.ExecutePendingJob.PendingJobActions.DeleteUserPicture;
public class DeleteUserPictureHandler : IRequestHandler<DeleteUserPictureRequest, DeleteUserPictureResponse>
{
    private readonly IUserPicturesRepository _userPicturesRepository;

    public DeleteUserPictureHandler(IUserPicturesRepository userPicturesRepository)
    {
        _userPicturesRepository = userPicturesRepository;
    }

    public async Task<DeleteUserPictureResponse> Handle(DeleteUserPictureRequest request, CancellationToken cancellationToken)
    {
        await _userPicturesRepository.DeleteAllUserSessionPictures(request.Payload.UserSessionGuid, cancellationToken);
        return new();
    }
}
