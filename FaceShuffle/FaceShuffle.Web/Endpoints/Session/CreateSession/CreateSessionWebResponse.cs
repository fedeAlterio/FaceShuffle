using FaceShuffle.Web.DTO;

namespace FaceShuffle.Web.Endpoints.Session.CreateSession;

public class CreateSessionWebResponse
{
    public required UserSessionDto UserSession { get; init; }
    public required string JsonWebToken { get; set; }
}
