using FaceShuffle.Models.Session;

namespace FaceShuffle.Models;

public class UserIdentity
{
    public required Username Username { get; init; }
    public required UserSessionGuid UserSessionGuid { get; init; }
}
