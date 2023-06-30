using FaceShuffle.Models.Session;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.InvalidateSession;
public class InvalidateSessionRequest : IRequest<InvalidateSessionResponse>
{
    public required UserSessionId UserSessionId { get; init; }
    public required UserSessionGuid UserSessionGuid { get; init; }
}
