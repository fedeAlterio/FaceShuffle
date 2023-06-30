using FaceShuffle.Application.Abstractions;
using FaceShuffle.Models.PendingJobs;
using FaceShuffle.Models.PendingJobs.Events;

namespace FaceShuffle.Application.Actions.PendingJobs;
public class PendingJobsService : IPendingJobService
{
    private readonly IAppDbContext _appDbContext;
    private readonly IDomainEventsCollector _publisher;

    public PendingJobsService(IAppDbContext appDbContext, IDomainEventsCollector publisher)
    {
        _appDbContext = appDbContext;
        _publisher = publisher;
    }

    public Task EnqueueNewJob(PendingJob job, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _appDbContext.PendingJobs.DbSet.Add(job);
        _publisher.AddNewEvent(new PendingJobAddedEvent(job));

        return Task.CompletedTask;
    }
}
