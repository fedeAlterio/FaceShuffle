using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Abstractions.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FaceShuffle.Infrastructure.Persistence;
public class AppDbContext : IAppDbContext
{
    private readonly RawAppDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    public AppDbContext(RawAppDbContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
    }

    private IUserSessionRepository? _userSessions;
    public IUserSessionRepository UserSessions => _userSessions ??= _serviceProvider.GetService<IUserSessionRepository>()!;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
