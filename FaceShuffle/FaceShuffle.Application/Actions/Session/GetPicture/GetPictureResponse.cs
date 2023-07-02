using FaceShuffle.Models.UserPictures;

namespace FaceShuffle.Application.Actions.Session.GetPicture;
public class GetPictureResponse
{
    public required Stream PictureStream { get; init; }
    public required UserPictureMetadata PictureMetadata { get; init; }
}
