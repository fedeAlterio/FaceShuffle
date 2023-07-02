using MediatR;

namespace FaceShuffle.Web.Endpoints.Session.CreateSession;

public class CreateSessionWebRequest : IRequest<CreateSessionWebResponse>
{
    public string Username { get; init; } = null!;
    public string Bio { get; init; } = null!;
    public int UserAge { get; init; }
    public string UserFullName { get; init; } = null!;
}
