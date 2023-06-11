using FaceShuffle.Application.Abstractions;
using FaceShuffle.Models.Events;
using MediatR;

namespace FaceShuffle.Application.EventHandlers.UserAuthenticated;
internal class UpdateUserSessionLastSeenDate : INotificationHandler<UserAuthenticatedEvent>
{
    private readonly IAppDbContext _appDbContext;

    public UpdateUserSessionLastSeenDate(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task Handle(UserAuthenticatedEvent notification, CancellationToken cancellationToken)
    {
        var userSession =
            await _appDbContext.UserSessions.GetActiveSessionByName(notification.UserIdentity.Name, cancellationToken);

        userSession.LastSeenDate = DateTime.UtcNow;
    }
}
