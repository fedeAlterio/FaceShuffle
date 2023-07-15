using FaceShuffle.Models;
using FaceShuffle.Models.Messages;
using FaceShuffle.Models.Session;
using MediatR;

namespace FaceShuffle.Application.Actions.Messages.SendMessage;
public class SendMessageRequest : IRequest<SendMessageResponse>
{
    public required UserIdentity Sender { get; init; }
    public required Username Receiver { get; init; }
    public required MessageTextContent MessageTextContent { get; init; }
}
