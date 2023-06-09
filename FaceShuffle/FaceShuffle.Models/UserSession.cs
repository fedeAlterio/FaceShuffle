namespace FaceShuffle.Models;
public class UserSession
{
    public const int NameMaximumLength = 20;
    public int Id { get; set; }
    public required Guid SessionGuid { get; set; }
    public required string Name { get; set; }
    public required DateTime CreationDate { get; set; }
    public required DateTime LastSeenDate { get; set; }
}
