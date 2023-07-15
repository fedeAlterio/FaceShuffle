using FaceShuffle.Web.Endpoints.Abstractions;
using FaceShuffle.Web.Endpoints.Generics;

namespace FaceShuffle.Web.Endpoints.Messages.SendMessage;
public class SendMessageEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder endpoints)
    {
        endpoints
            .MapPost(@"/SendMessage", MediatorEndpoint.FromBody<SendMessageWebRequest, SendMessageWebResponse>)
            .WithName("SendMessage");
    }
}
