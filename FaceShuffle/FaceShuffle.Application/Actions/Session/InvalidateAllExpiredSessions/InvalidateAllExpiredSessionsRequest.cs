using MediatR;

namespace FaceShuffle.Application.Actions.Session.InvalidateAllExpiredSessions;
public class InvalidateAllExpiredSessionsRequest : IRequest<InvalidateAllExpiredSessionsResponse>
{
}
