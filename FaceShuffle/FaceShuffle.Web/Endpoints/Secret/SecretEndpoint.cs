using System.Security.Claims;
using FaceShuffle.Application.Abstractions.Auth;
using FaceShuffle.Web.Endpoints.Abstractions;
using FaceShuffle.Web.Endpoints.Utilities;

namespace FaceShuffle.Web.Endpoints.Secret;

public class SecretEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/secret", (ClaimsPrincipal claims, IAuthService authorizationService) =>
        {
            var identity = authorizationService.GetUserIdentity(claims.Identity);
            return identity;
        })
        .RequireAuthorization()
        .WithDefaultConfiguration();
    }
}
