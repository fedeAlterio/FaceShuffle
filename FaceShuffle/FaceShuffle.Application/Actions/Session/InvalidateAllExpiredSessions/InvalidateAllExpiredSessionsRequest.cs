using FaceShuffle.Application.PipelineBehaviors;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.InvalidateAllExpiredSessions;
public class InvalidateAllExpiredSessionsRequest : IRequest<InvalidateAllExpiredSessionsResponse>, INotUnitOfWorkRequest
{
}
