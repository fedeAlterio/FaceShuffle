using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.Messages.SendMessage;
using FaceShuffle.Models;
using MediatR;

namespace FaceShuffle.Web.Endpoints.Messages.SendMessage;
public class SendMessageWebHandler : IRequestHandler<SendMessageWebRequest, SendMessageWebResponse>
{
    private readonly UserIdentity _userIdentity;
    private readonly IRequestSender<SendMessageRequest, SendMessageResponse> _requestSender;

    public SendMessageWebHandler(UserIdentity userIdentity, IRequestSender<SendMessageRequest, SendMessageResponse> requestSender)
    {
        _userIdentity = userIdentity;
        _requestSender = requestSender;
    }

    public async Task<SendMessageWebResponse> Handle(SendMessageWebRequest webRequest, CancellationToken cancellationToken)
    {
        var request = new SendMessageRequest
        {
            MessageTextContent = new(webRequest.MessageTextContent),
            Sender = _userIdentity,
            Receiver = new(webRequest.Username)
        };

        await _requestSender.Send(request, cancellationToken);

        return new();
    }
}
