namespace FaceShuffle.Models.Session;
public class UserSession
{
    public UserSessionId Id { get; init; }
    public required Guid SessionGuid { get; init; }
    public required Username Username { get; init; }
    public required DateTime CreationDate { get; init; }
    public required int MinutesBeforeExpiration { get; set; }
    public required DateTime LastSeenDate { get; set; }
}
