using MediatR;

namespace FaceShuffle.Web.Endpoints.CreateSession;

public class CreateSessionWebRequest : IRequest<CreateSessionWebResponse>
{
    public required string Name { get; init; }
}
