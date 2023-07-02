using FaceShuffle.Application.Repositories;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.GetPicture;
public class GetPictureHandler : IRequestHandler<GetPictureRequest, GetPictureResponse>
{
    private readonly IUserPicturesRepository _userPicturesRepository;

    public GetPictureHandler(IUserPicturesRepository userPicturesRepository)
    {
        _userPicturesRepository = userPicturesRepository;
    }

    public async Task<GetPictureResponse> Handle(GetPictureRequest request, CancellationToken cancellationToken)
    {
        var (stream, metadata) = await _userPicturesRepository.LoadUserPictureStream(request.UserSessionGuid, request.UserPictureGuid, cancellationToken);
        return new()
        {
            PictureStream = stream,
            PictureMetadata = metadata
        };
    }
}
