using FaceShuffle.Models.Session;

namespace FaceShuffle.Models.UserPictures;
public class UserPictureMetadata
{
    public required UserSessionGuid SessionGuid { get; init; }
    public required UserPictureGuid UserPictureGuid { get; init; }
    public required UserPictureFileName FileName { get; init; }
    public string ContentType => MimeTypes.GetMimeType(FileName.Value);
}
