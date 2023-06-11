using FaceShuffle.Web.Endpoints.Abstractions;
using FaceShuffle.Web.Endpoints.Generics;

namespace FaceShuffle.Web.Endpoints.Session.CreateSession;

public class CreateSessionEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder endpoints)
    {
        endpoints.MapPost(@"/NewSession", MediatorEndpoint.Handle<CreateSessionWebRequest, CreateSessionWebResponse>)
                        .WithName("New session")
                        .WithDefaultEndpointConfiguration();
    }
}
