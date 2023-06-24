using FaceShuffle.Application.PipelineBehaviors;
using FaceShuffle.Models.Session;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.CreateSession;
public class CreateSessionRequest : IRequest<CreateSessionResponse>, IUnitOfWorkRequest
{
    public required Username Username { get; init; }
}
