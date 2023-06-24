using MediatR;

namespace FaceShuffle.Application.Actions.Session.InvalidateSession;
public class InvalidateSessionRequest : IRequest<InvalidateSessionResponse>
{
    public required int SessionId { get; init; }
}
