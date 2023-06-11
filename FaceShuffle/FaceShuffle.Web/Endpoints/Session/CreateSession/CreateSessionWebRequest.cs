using MediatR;

namespace FaceShuffle.Web.Endpoints.Session.CreateSession;

public class CreateSessionWebRequest : IRequest<CreateSessionWebResponse>
{
    public required string Name { get; init; }
}
