using MediatR;

namespace FaceShuffle.Web.Endpoints.Session.CreateSession;

public class CreateSessionWebRequest : IRequest<CreateSessionWebResponse>
{
    public required string Username { get; init; }
    public required string Bio { get; init; }
    public required int UserAge { get; init; }
    public required string UserFullName { get; init; }
}
