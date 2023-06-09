using FaceShuffle.Web.Endpoints.Abstractions;
using FaceShuffle.Web.Endpoints.Utilities;

namespace FaceShuffle.Web.Endpoints.Secret;

public class SecretEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/secret", EndpointHandlers.AuthorizedFrom<SecretWebRequest, SecretWebResponse>())
        .RequireAuthorization()
        .WithDefaultConfiguration();
    }
}
