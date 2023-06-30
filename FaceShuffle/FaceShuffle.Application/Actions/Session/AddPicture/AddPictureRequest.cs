using FaceShuffle.Models.Session;
using FaceShuffle.Models.UserPictures;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.AddPicture;
public class AddPictureRequest : IRequest<AddPictureResponse>
{
    public required UserSessionGuid SessionGuid { get; init; }
    public required UserPictureFileName FileName { get; init; }
    public required UserPictureLength UserPictureSizeBytes { get; init; }
    public required Stream PictureStream { get; init; }
}
