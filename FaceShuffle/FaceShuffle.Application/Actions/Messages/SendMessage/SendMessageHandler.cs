using FaceShuffle.Application.Abstractions;
using FaceShuffle.Models.Messages;
using MediatR;

namespace FaceShuffle.Application.Actions.Messages.SendMessage;
public class SendMessageHandler : IRequestHandler<SendMessageRequest, SendMessageResponse>
{
    private readonly IAppDbContext _appDbContext;

    public SendMessageHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<SendMessageResponse> Handle(SendMessageRequest request, CancellationToken cancellationToken)
    {
        var receiverSession = await _appDbContext.UserSessions.FindSessionByUsername(request.Receiver, cancellationToken);

        var message = new Message
        {
            Sender = request.Sender.UserSessionGuid,
            Receiver = receiverSession.SessionGuid,
            MessageTextContent = request.MessageTextContent,
            SentDate = DateTime.UtcNow
        };

        _appDbContext.Messages.DbSet.Add(message);

        return new();
    }
}
