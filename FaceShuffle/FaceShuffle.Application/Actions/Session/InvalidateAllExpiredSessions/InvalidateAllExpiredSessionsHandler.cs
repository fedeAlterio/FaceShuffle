using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.Session.InvalidateSession;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceShuffle.Application.Actions.Session.InvalidateAllExpiredSessions;
public class InvalidateAllExpiredSessionsHandler : IRequestHandler<InvalidateAllExpiredSessionsRequest, InvalidateAllExpiredSessionsResponse>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IRequestSender<InvalidateSessionRequest, InvalidateSessionResponse> _invalidateSession;

    public InvalidateAllExpiredSessionsHandler(
        IAppDbContext appDbContext, 
        IRequestSender<InvalidateSessionRequest, InvalidateSessionResponse> invalidateSession)
    {
        _appDbContext = appDbContext;
        _invalidateSession = invalidateSession;
    }

    public async Task<InvalidateAllExpiredSessionsResponse> Handle(InvalidateAllExpiredSessionsRequest request, CancellationToken cancellationToken)
    {
        var sessions = _appDbContext.UserSessions;
        var expiredSessions = await sessions.DbSet
            .Where(x => x.LastSeenDate.AddMinutes(x.MinutesBeforeExpiration) < DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var expiredSession in expiredSessions)
        {
            var invalidateRequest = new InvalidateSessionRequest
            {
                SessionId = expiredSession.Id
            };

            await _invalidateSession.Send(invalidateRequest, cancellationToken);
        }
        return new();
    }
}
