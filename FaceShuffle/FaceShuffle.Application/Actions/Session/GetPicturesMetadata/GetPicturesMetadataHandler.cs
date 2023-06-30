using FaceShuffle.Application.Repositories;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.GetPicturesMetadata;
public class GetPicturesMetadataHandler : IRequestHandler<GetPicturesMetadataRequest, GetPicturesMetadataResponse>
{
    private readonly IUserPicturesRepository _userPicturesRepository;

    public GetPicturesMetadataHandler(IUserPicturesRepository userPicturesRepository)
    {
        _userPicturesRepository = userPicturesRepository;
    }

    public async Task<GetPicturesMetadataResponse> Handle(GetPicturesMetadataRequest request, CancellationToken cancellationToken)
    {
        var metadata = await _userPicturesRepository.LoadUserPicturesMetadata(request.UserSessionGuid, cancellationToken);
        return new()
        {
            PicturesMetadata = metadata.ToList()
        };
    }
}
