using FaceShuffle.Application.Configuration;
using FaceShuffle.Application.Exceptions;
using FaceShuffle.Application.Repositories;
using FaceShuffle.Models.Session;
using FaceShuffle.Models.UserPictures;

namespace FaceShuffle.Infrastructure.Persistence;
public class UserPictureFileSystemRepository : IUserPicturesRepository
{
    private readonly UserPicturesConfiguration _userPicturesConfiguration;

    public UserPictureFileSystemRepository(UserPicturesConfiguration userPicturesConfiguration)
    {
        _userPicturesConfiguration = userPicturesConfiguration;
    }

    public async Task<UserPictureMetadata> SaveUserPicture(UserSessionGuid userSessionGuid, UserPictureFileName fileName, Stream picturesStream,
        CancellationToken cancellationToken)
    {
        var metadata = new UserPictureMetadata
        {
            FileName = fileName,
            SessionGuid = userSessionGuid,
            UserPictureGuid = new(Guid.NewGuid())
        };

        await SaveUserPicture(metadata, picturesStream, cancellationToken);
        return metadata;
    }

    public Task<IEnumerable<UserPictureMetadata>> LoadUserPicturesMetadata(UserSessionGuid userSessionGuid, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var sessionDirectoryPath = GetPicturesDirectoryPathForSession(userSessionGuid);
        if(!Directory.Exists(sessionDirectoryPath))
            return Task.FromResult(Enumerable.Empty<UserPictureMetadata>());

        var picturesMetadata = Directory
            .GetFiles(sessionDirectoryPath)
            .Select(path => GetPictureMetadataFromFileName(userSessionGuid, path));

        return Task.FromResult(picturesMetadata);
    }

    public Task<(Stream pictureStream, UserPictureMetadata pictureMetadata)> LoadUserPictureStream(UserSessionGuid sessionGuid, UserPictureGuid pictureGuid, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var picturesPath = GetPicturesDirectoryPathForSession(sessionGuid);
        var pictureGuidString = pictureGuid.Value.ToString();
        var picturePath = Directory
                           .EnumerateFiles(picturesPath + "\\")
                           .FirstOrDefault(path => Path.GetFileName(path).StartsWith(pictureGuidString))
                       ?? throw new UserReadableAppException
                       {
                           UserText = $"Picture with id {pictureGuidString} not found"
                       };

        Stream pictureStream = File.Open(picturePath, FileMode.Open);
        var pictureMetadata = GetPictureMetadataFromFileName(sessionGuid, picturePath);

        return Task.FromResult((pictureStream, pictureMetadata));
    }

    public Task DeleteAllUserSessionPictures(UserSessionGuid userSessionGuid, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var directory = GetPicturesDirectoryPathForSession(userSessionGuid);
        if(!Directory.Exists(directory))
            return Task.CompletedTask;

        Directory.Delete(directory, true);

        return Task.CompletedTask;
    }

    public Task DeleteUserPicture(UserPictureMetadata userPictureMetadata, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var picturePath = GetPictureFullPath(userPictureMetadata);
        File.Delete(picturePath);

        return Task.CompletedTask;
    }

    async Task SaveUserPicture(UserPictureMetadata userPictureMetadata, Stream picturesStream,
        CancellationToken cancellationToken)
    {
        var filePath = GetPictureFullPath(userPictureMetadata);

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        await using var file = File.Create(filePath);
        await picturesStream.CopyToAsync(file, cancellationToken);
    }

    string GetPicturesDirectoryPathForSession(UserSessionGuid userSessionGuid)
    {
        return Path.Combine(_userPicturesConfiguration.Root, GetPictureFolderName(userSessionGuid));
    }

    static string GetPictureFolderName(UserSessionGuid userSessionGuid)
    {
        return $"{userSessionGuid.Value}";
    }

    static string GetFileName(UserPictureMetadata userPictureMetadata)
    {
        return $"{userPictureMetadata.UserPictureGuid.Value}_{userPictureMetadata.FileName.Value}";
    }

    private string GetPictureFullPath(UserPictureMetadata userPictureMetadata)
    {
        return Path.Combine(GetPicturesDirectoryPathForSession(userPictureMetadata.SessionGuid), GetFileName(userPictureMetadata));
    }

    UserPictureMetadata GetPictureMetadataFromFileName(UserSessionGuid userSessionGuid, string path)
    {
        var rawFileName = Path.GetFileName(path);
        var metadataSplit = rawFileName.Split('_');
        var pictureGuid = new UserPictureGuid(Guid.Parse(metadataSplit[0]));
        var fileName = new UserPictureFileName(metadataSplit[1]);

        return new()
        {
            UserPictureGuid = pictureGuid,
            FileName = fileName,
            SessionGuid = userSessionGuid
        };
    }
}
