using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
    public IUserSessionRepository UserSessions => _userSessions ??= _serviceProvider.GetRequiredService<IUserSessionRepository>();


    private IPendingJobsRepository? _pendingJobs;
    public IPendingJobsRepository PendingJobs => _pendingJobs ??= _serviceProvider.GetRequiredService<IPendingJobsRepository>();


    private IMessagesRepository? _messages;
    public IMessagesRepository Messages => _messages ??= _serviceProvider.GetRequiredService<IMessagesRepository>();


    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ApplyMigrations(CancellationToken cancellationToken)
    {
        await _dbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }
}
