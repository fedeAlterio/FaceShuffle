using FaceShuffle.Web.Endpoints.Abstractions;
using FaceShuffle.Web.Endpoints.Utilities;

namespace FaceShuffle.Web.Endpoints.CreateSession;

public class CreateSessionEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(@"/NewSession", EndpointHandlers.From<CreateSessionWebRequest, CreateSessionWebResponse>())
                        .WithName("New session")
                        .WithDefaultConfiguration();
    }
}
