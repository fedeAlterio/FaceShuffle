using FaceShuffle.Application.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace FaceShuffle.Application.Abstractions;
public interface IAppDbContext
{
    IUserSessionRepository UserSessions { get; }
    IPendingJobsRepository PendingJobs { get; }
    IMessagesRepository Messages { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task ApplyMigrations(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}
