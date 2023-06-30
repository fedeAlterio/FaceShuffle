using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Abstractions.Auth;
using FaceShuffle.Models.Events;
using MediatR;
using Microsoft.Extensions.Options;

namespace FaceShuffle.Application.Actions.Session.EventHandlers;
internal class UpdateUserSessionLastSeenDate : INotificationHandler<UserAuthenticatedEvent>
{
    private readonly IAppDbContext _appDbContext;
    private readonly UserSessionConfiguration _userSessionConfiguration;

    public UpdateUserSessionLastSeenDate(
        IAppDbContext appDbContext,
        IOptions<UserSessionConfiguration> userSessionConfiguration)
    {
        _appDbContext = appDbContext;
        _userSessionConfiguration = userSessionConfiguration.Value;
    }

    public async Task Handle(UserAuthenticatedEvent notification, CancellationToken cancellationToken)
    {
        var userSession =
            await _appDbContext.UserSessions.GetActiveSessionByUsername(notification.UserIdentity.Username, cancellationToken);

        userSession.LastSeenDate = DateTime.UtcNow;
        userSession.MinutesBeforeExpiration = _userSessionConfiguration.MinutesBeforeExpiration;
    }
}
