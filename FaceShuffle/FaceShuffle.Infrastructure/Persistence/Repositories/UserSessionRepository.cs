using FaceShuffle.Application.Abstractions.Repositories;
using FaceShuffle.Application.Extensions;
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
    public async Task<UserSession> GetActiveSessionByUsername(Username username, CancellationToken cancellationToken)
    {
        return await DbSet.FirstAsync(x => x.Username == username, cancellationToken);
    }

    public Task<UserSession> FindSessionById(UserSessionId userSessionId, CancellationToken cancellationToken)
    {
        return DbSet.FindAsyncOrThrow(new object[] { userSessionId }, cancellationToken);
    }

    public Task<bool> ExistsUsername(Username username, CancellationToken cancellationToken)
    {
        return DbSet.AnyAsync(x => x.Username == username, cancellationToken);
    }
}
