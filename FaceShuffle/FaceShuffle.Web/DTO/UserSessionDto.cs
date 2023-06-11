using FaceShuffle.Models;

namespace FaceShuffle.Web.DTO;


public static class UserSessionDtoMap
{
    public static UserSessionDto ToUserSessionDto(this UserSession @this) => UserSessionDto.From(@this);
}

public class UserSessionDto
{
    public static UserSessionDto From(UserSession userSession)
    {
        var ret = new UserSessionDto
        {
            Username = userSession.Username,
            CreationDate = userSession.CreationDate,
            Name = userSession.Name
        };

        return ret;
    }

    public required string Username { get; init; }
    public required string Name { get; init; }
    public required DateTime CreationDate { get; init; }
}
