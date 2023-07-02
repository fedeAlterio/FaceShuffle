namespace FaceShuffle.Web.Endpoints.Session.GetUserProfile;
public class GetUserProfileWebResponse
{
    public required Guid SessionGuid { get; init; }
    public required string Username { get; init; }
    public required int UserAge { get; init; }
    public required string Bio { get; init; }
    public required string UserFullName { get; init; }
    public required DateTime CreationDate { get; init; }
    public required DateTime LastSeenDate { get; set; }
}
