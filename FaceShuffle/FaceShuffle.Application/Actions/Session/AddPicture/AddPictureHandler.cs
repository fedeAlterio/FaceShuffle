using FaceShuffle.Application.Repositories;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.AddPicture;
public class AddPictureHandler : IRequestHandler<AddPictureRequest, AddPictureResponse>
{
    private readonly IUserPicturesRepository _picturesRepository;

    public AddPictureHandler(IUserPicturesRepository picturesRepository)
    {
        _picturesRepository = picturesRepository;
    }

    public async Task<AddPictureResponse> Handle(AddPictureRequest request, CancellationToken cancellationToken)
    {
        var metadata = await _picturesRepository.SaveUserPicture(
            request.SessionGuid, 
            request.FileName, 
            request.PictureStream,
            cancellationToken);

        return new()
        {
            PictureMetadata = metadata,
        };
    }
}
