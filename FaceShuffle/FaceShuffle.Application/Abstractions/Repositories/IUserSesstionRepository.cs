using FaceShuffle.Models.Session;
using Microsoft.EntityFrameworkCore;

namespace FaceShuffle.Application.Abstractions.Repositories;
public interface IUserSessionRepository
{
    DbSet<UserSession> DbSet { get; }
    Task<UserSession> GetActiveSessionByUsername(Username username, CancellationToken cancellationToken);
    Task<UserSession> FindSessionById(int userSessionId, CancellationToken cancellationToken);
    Task<bool> ExistsUsername(Username username, CancellationToken cancellationToken);
}
