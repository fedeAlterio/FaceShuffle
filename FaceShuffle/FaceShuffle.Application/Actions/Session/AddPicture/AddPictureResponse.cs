using FaceShuffle.Models.UserPictures;

namespace FaceShuffle.Application.Actions.Session.AddPicture;
public class AddPictureResponse
{
    public required UserPictureMetadata PictureMetadata { get; init; }
}
