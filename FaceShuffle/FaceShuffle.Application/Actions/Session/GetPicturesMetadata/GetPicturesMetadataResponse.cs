using FaceShuffle.Models.UserPictures;

namespace FaceShuffle.Application.Actions.Session.GetPicturesMetadata;
public class GetPicturesMetadataResponse
{
    public required IReadOnlyList<UserPictureMetadata> PicturesMetadata { get; init; }
}
