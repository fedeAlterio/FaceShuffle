using FaceShuffle.Models.Session;

namespace FaceShuffle.Application.Actions.Session.CreateSession;
public class CreateSessionResponse
{
    public required UserSession UserSession { get; init; }
    public required string JsonWebToken { get; set; }
}
