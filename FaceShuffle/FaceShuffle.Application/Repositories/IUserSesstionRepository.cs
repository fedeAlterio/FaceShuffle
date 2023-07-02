using FaceShuffle.Models.Session;
using Microsoft.EntityFrameworkCore;

namespace FaceShuffle.Application.Repositories;
public interface IUserSessionRepository
{
    DbSet<UserSession> DbSet { get; }
    Task<UserSession> FindSessionByUsername(Username username, CancellationToken cancellationToken);
    Task<UserSession> FindSessionById(UserSessionId userSessionId, CancellationToken cancellationToken);
    Task<UserSession> FindSessionByGuid(UserSessionGuid userSessionId, CancellationToken cancellationToken);
    Task<bool> ExistsUsername(Username username, CancellationToken cancellationToken);
}
