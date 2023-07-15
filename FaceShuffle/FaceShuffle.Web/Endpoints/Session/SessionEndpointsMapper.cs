using FaceShuffle.Web.Endpoints.Session.AddUserPicture;
using FaceShuffle.Web.Endpoints.Session.CreateSession;
using FaceShuffle.Web.Endpoints.Session.GetPicture;
using FaceShuffle.Web.Endpoints.Session.GetPicturesMetadata;
using FaceShuffle.Web.Endpoints.Session.GetUserProfile;
using FaceShuffle.Web.Endpoints.Session.UpdateUserProfile;

namespace FaceShuffle.Web.Endpoints.Session;

public static class SessionEndpointsMapper
{
    public static RouteGroupBuilder MapSessionEndpoints(this RouteGroupBuilder @this, IServiceProvider serviceProvider)
    {
        @this.MapGroup("Session")
            .MapEndpoint<CreateSessionEndpoint>(serviceProvider)
            .MapEndpoint<AddUserPictureEndpoint>(serviceProvider)
            .MapEndpoint<GetPicturesMetadataEndpoint>(serviceProvider)
            .MapEndpoint<GetPictureEndpoint>(serviceProvider)
            .MapEndpoint<GetUserProfileEndpoint>(serviceProvider)
            .MapEndpoint<UpdateUserProfileEndpoint>(serviceProvider);

        return @this;
    }
}
