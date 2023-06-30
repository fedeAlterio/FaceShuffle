using Microsoft.EntityFrameworkCore;
using FaceShuffle.Models.PendingJobs;

namespace FaceShuffle.Application.Repositories;
public interface IPendingJobsRepository
{
    DbSet<PendingJob> DbSet { get; }
}
