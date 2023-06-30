using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.Session.InvalidateSession;
using FaceShuffle.Application.Helpers;
using FaceShuffle.Models.Generic;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FaceShuffle.Application.Actions.Session.InvalidateAllExpiredSessions;
public class InvalidateAllExpiredSessionsHandler : IRequestHandler<InvalidateAllExpiredSessionsRequest, InvalidateAllExpiredSessionsResponse>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IRequestSender<InvalidateSessionRequest, InvalidateSessionResponse> _invalidateSession;
    private readonly ILogger<InvalidateAllExpiredSessionsHandler> _logger;

    public InvalidateAllExpiredSessionsHandler(
        IAppDbContext appDbContext, 
        IRequestSender<InvalidateSessionRequest, InvalidateSessionResponse> invalidateSession,
        ILogger<InvalidateAllExpiredSessionsHandler> logger)
    {
        _appDbContext = appDbContext;
        _invalidateSession = invalidateSession;
        _logger = logger;
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
                UserSessionId = expiredSession.Id,
                UserSessionGuid = expiredSession.SessionGuid,
            };

            await TryEx
                .Try(() => _invalidateSession.Send(invalidateRequest, cancellationToken))
                .OnSome(exception => _logger.LogError(exception, "Failed to invalidate session {expiredSession.SessionGuid}", expiredSession));
        }

        return new();
    }
}
