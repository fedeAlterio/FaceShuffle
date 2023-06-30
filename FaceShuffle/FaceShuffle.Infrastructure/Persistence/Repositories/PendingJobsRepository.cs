using FaceShuffle.Application.Repositories;
using FaceShuffle.Models.PendingJobs;
using Microsoft.EntityFrameworkCore;

namespace FaceShuffle.Infrastructure.Persistence.Repositories;
public class PendingJobsRepository : IPendingJobsRepository
{
    private readonly RawAppDbContext _appDbContext;

    public PendingJobsRepository(RawAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public DbSet<PendingJob> DbSet => _appDbContext.PendingJobs;
}
