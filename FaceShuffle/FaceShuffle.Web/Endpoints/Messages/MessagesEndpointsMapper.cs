using FaceShuffle.Web.Endpoints.Messages.SendMessage;

namespace FaceShuffle.Web.Endpoints.Messages;

public static class MessagesEndpointsMapper
{
    public static RouteGroupBuilder MapMessagesEndpoints(this RouteGroupBuilder @this, IServiceProvider serviceProvider)
    {
        @this.MapGroup("Messages")
            .RequireAuthorization()
            .MapEndpoint<SendMessageEndpoint>(serviceProvider);

        return @this;
    }
}
