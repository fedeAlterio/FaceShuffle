namespace FaceShuffle.Models;
public class UserSession
{
    public const int NameMaximumLength = 20;
    public int Id { get; init; }
    public required string Username { get; init; }
    public required string Name { get; init; }
    public required DateTime CreationDate { get; init; }
    public required DateTime LastSeenDate { get; set; }
}
