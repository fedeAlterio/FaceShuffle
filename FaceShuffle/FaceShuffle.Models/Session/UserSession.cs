namespace FaceShuffle.Models.Session;
public class UserSession
{
    public UserSessionId Id { get; init; }
    public required UserSessionGuid SessionGuid { get; init; }
    public required Username Username { get; set; }
    public required UserAge UserAge { get; set; }
    public required Bio Bio { get; set; }
    public required UserFullName UserFullName { get; set; }
    public required DateTime CreationDate { get; init; }
    public required int MinutesBeforeExpiration { get; set; }
    public required DateTime LastSeenDate { get; set; }
}
