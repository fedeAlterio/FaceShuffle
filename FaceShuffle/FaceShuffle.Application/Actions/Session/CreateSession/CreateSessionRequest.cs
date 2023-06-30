using FaceShuffle.Models.Session;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.CreateSession;
public class CreateSessionRequest : IRequest<CreateSessionResponse>
{
    public required Username Username { get; init; }
    public required Bio Bio { get; init; }
    public required UserAge UserAge { get; init; }
    public required UserFullName UserFullName { get; init; }
}
