using FaceShuffle.Models.UserPictures;

namespace FaceShuffle.Web.DTO;

public class UserPictureMetadataDto
{
    public required Guid UserPictureGuid { get; init; }
    public required string FileName { get; init; }

    public static UserPictureMetadataDto FromUserPictureMetadata(UserPictureMetadata userPictureMetadata)
    {
        return new()
        {
            UserPictureGuid = userPictureMetadata.UserPictureGuid.Value,
            FileName = userPictureMetadata.FileName.Value,
        };
    }
}
