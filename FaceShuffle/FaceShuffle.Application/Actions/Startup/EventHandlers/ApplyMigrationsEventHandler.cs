using FaceShuffle.Application.Abstractions;
using FaceShuffle.Models.Events;
using MediatR;

namespace FaceShuffle.Application.Actions.Startup.EventHandlers;
public class ApplyMigrationsEventHandler : INotificationHandler<ApplicationStartingEvent>
{
    private readonly IAppDbContext _appDbContext;

    public ApplyMigrationsEventHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task Handle(ApplicationStartingEvent notification, CancellationToken cancellationToken)
    {
        await _appDbContext.ApplyMigrations(cancellationToken);
    }
}
