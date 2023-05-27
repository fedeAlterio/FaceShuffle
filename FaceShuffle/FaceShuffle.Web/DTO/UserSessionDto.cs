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
            SessionGuid = userSession.SessionGuid,
            CreationDate = userSession.CreationDate,
            Name = userSession.Name
        };

        return ret;
    }

    public required Guid SessionGuid { get; set; }
    public required string Name { get; set; }
    public required DateTime CreationDate { get; set; }
}
