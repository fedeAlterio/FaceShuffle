using FaceShuffle.Application.Exceptions;
using FaceShuffle.Application.Extensions;
using FaceShuffle.Application.Repositories;
using FaceShuffle.Models.Session;
using Microsoft.EntityFrameworkCore;

namespace FaceShuffle.Infrastructure.Persistence.Repositories;
public class UserSessionRepository : IUserSessionRepository
{
    private readonly RawAppDbContext _dbContext;

    public UserSessionRepository(RawAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DbSet<UserSession> DbSet => _dbContext.UserSessions;
    public async Task<UserSession> FindSessionByUsername(Username username, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.Username == username, cancellationToken)
            ?? throw SessionNotExistsException.Create(username);
    }

    public Task<UserSession> FindSessionById(UserSessionId userSessionId, CancellationToken cancellationToken)
    {
        return DbSet.FindAsyncOrThrow(new object[] { userSessionId }, cancellationToken);
    }

    public async Task<UserSession> FindSessionByGuid(UserSessionGuid userSessionGuid, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(x => x.SessionGuid == userSessionGuid, cancellationToken)
                   ?? throw SessionNotExistsException.Create(userSessionGuid);
    }

    public Task<bool> ExistsUsername(Username username, CancellationToken cancellationToken)
    {
        return DbSet.AnyAsync(x => x.Username == username, cancellationToken);
    }
}
