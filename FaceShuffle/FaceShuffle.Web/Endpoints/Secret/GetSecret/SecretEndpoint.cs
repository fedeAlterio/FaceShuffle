using FaceShuffle.Web.Endpoints.Abstractions;
using FaceShuffle.Web.Endpoints.Generics;

namespace FaceShuffle.Web.Endpoints.Secret;

public class SecretEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost("/", MediatorEndpoint.FromBody<SecretWebRequest, SecretWebResponse>);
    }
}
