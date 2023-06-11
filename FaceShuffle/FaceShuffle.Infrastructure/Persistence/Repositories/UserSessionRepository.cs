using FaceShuffle.Application.Abstractions.Repositories;
using FaceShuffle.Models;
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
    public async Task<UserSession> GetActiveSessionByName(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.UserSessions.FirstAsync(x => x.Name == name, cancellationToken);
    }
}
