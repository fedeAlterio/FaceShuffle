using FaceShuffle.Web.Endpoints.Abstractions;
using FaceShuffle.Web.Endpoints.Generics;

namespace FaceShuffle.Web.Endpoints.Session.UpdateUserProfile;
public class UpdateUserProfileEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder endpoints)
    {
        endpoints
            .MapPost(@"/UpdateProfile", MediatorEndpoint.FromBody<UpdateUserProfileWebRequest, UpdateUserProfileWebResponse>)
            .WithName("Update user profile");
    }
}
