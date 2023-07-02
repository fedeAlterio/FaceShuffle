using FaceShuffle.Web.Endpoints.Abstractions;
using FaceShuffle.Web.Endpoints.Generics;

namespace FaceShuffle.Web.Endpoints.Session.GetUserProfile;
public class GetUserProfileEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder endpoints)
    {
        endpoints
            .MapPost(@"/Profile", MediatorEndpoint.FromBody<GetUserProfileWebRequest, GetUserProfileWebResponse>)
            .WithName("GetUserProfile")
            .RequireAuthorization();
    }
}
