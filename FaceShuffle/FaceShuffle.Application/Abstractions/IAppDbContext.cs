using FaceShuffle.Application.Abstractions.Repositories;

namespace FaceShuffle.Application.Abstractions;
public interface IAppDbContext
{
    IUserSessionRepository UserSessions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
