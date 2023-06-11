using FaceShuffle.Web.Endpoints.Session.CreateSession;

namespace FaceShuffle.Web.Endpoints.Session;

public static class SessionEndpointsInstaller
{
    public static RouteGroupBuilder MapSessionEndpoints(this RouteGroupBuilder @this, IServiceProvider serviceProvider)
    {
        @this.MapGroup("Session")
             .MapEndpoint<CreateSessionEndpoint>(serviceProvider);

        return @this;
    }
}
