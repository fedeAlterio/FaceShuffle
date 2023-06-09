using FaceShuffle.Application.Abstractions;
using FaceShuffle.Models.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        await _appDbContext.UserSessions.DbSet
            .Where(session => session.Name == notification.UserIdentity.Name)
            .ExecuteUpdateAsync(x => x.SetProperty(session => session.LastSeenDate, DateTime.UtcNow), CancellationToken.None);
    }
}
