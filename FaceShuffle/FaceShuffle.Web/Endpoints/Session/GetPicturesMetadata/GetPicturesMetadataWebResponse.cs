using FaceShuffle.Web.DTO;

namespace FaceShuffle.Web.Endpoints.Session.GetPicturesMetadata;
public class GetPicturesMetadataWebResponse
{
    public required IReadOnlyList<UserPictureMetadataDto> PicturesMetadata { get; init; }
}
