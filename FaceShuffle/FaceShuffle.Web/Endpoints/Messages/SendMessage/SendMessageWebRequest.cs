using MediatR;

namespace FaceShuffle.Web.Endpoints.Messages.SendMessage;
public class SendMessageWebRequest : IRequest<SendMessageWebResponse>
{
    public string Username { get; init; } = default!;
    public string MessageTextContent { get; init; } = default!;
}
