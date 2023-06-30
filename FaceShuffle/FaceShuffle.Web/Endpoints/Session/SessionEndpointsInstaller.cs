using FaceShuffle.Web.Endpoints.Session.AddUserPicture;
using FaceShuffle.Web.Endpoints.Session.CreateSession;
using FaceShuffle.Web.Endpoints.Session.GetPicturesMetadata;

namespace FaceShuffle.Web.Endpoints.Session;

public static class SessionEndpointsInstaller
{
    public static RouteGroupBuilder MapSessionEndpoints(this RouteGroupBuilder @this, IServiceProvider serviceProvider)
    {
        @this.MapGroup("Session")
            .MapEndpoint<CreateSessionEndpoint>(serviceProvider)
            .MapEndpoint<AddUserPictureEndpoint>(serviceProvider)
            .MapEndpoint<GetPicturesMetadataEndpoint>(serviceProvider);

        return @this;
    }
}
