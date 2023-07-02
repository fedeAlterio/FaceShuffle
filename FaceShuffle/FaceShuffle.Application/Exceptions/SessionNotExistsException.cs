using FaceShuffle.Models.Session;

namespace FaceShuffle.Application.Exceptions;
public class SessionNotExistsException : UserReadableAppException
{
    SessionNotExistsException()
    {
    }

    public static SessionNotExistsException Create(UserSessionGuid guid) => new()
    {
        UserText = $"Session with guid {guid.Value} does not exist"
    };

    public static SessionNotExistsException Create(Username username) => new()
    {
        UserText = $"Session with username {username.Value} does not exist"
    };
}
