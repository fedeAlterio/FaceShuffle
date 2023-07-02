using FaceShuffle.Models.Session;
using FaceShuffle.Models.UserPictures;

namespace FaceShuffle.Application.Repositories;
public interface IUserPicturesRepository
{
    Task<UserPictureMetadata> SaveUserPicture(
        UserSessionGuid userSessionGuid,
        UserPictureFileName fileName,
        Stream picturesStream,
        CancellationToken cancellationToken);

    Task<IEnumerable<UserPictureMetadata>> LoadUserPicturesMetadata(UserSessionGuid userSessionGuid, CancellationToken cancellationToken);
    Task<(Stream pictureStream, UserPictureMetadata pictureMetadata)> LoadUserPictureStream(UserSessionGuid sessionGuid, UserPictureGuid pictureGuid, CancellationToken cancellationToken);
    Task DeleteAllUserSessionPictures(UserSessionGuid userSessionGuid, CancellationToken cancellationToken);
    Task DeleteUserPicture(UserPictureMetadata userPictureMetadata, CancellationToken cancellationToken);
}
