using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.PendingJobs.ExecutePendingJob;
using FaceShuffle.Models.PendingJobs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceShuffle.Application.Actions.PendingJobs.ExecuteAllPendingJobs;
public class ExecuteAllPendingJobsHandler : IRequestHandler<ExecuteAllPendingJobsRequest, ExecuteAllPendingJobsResponse>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IRequestSender<ExecutePendingJobRequest, ExecutePendingJobResponse> _executePendingJob;

    public ExecuteAllPendingJobsHandler(IAppDbContext appDbContext,IRequestSender<ExecutePendingJobRequest, ExecutePendingJobResponse> executePendingJob)
    {
        _appDbContext = appDbContext;
        _executePendingJob = executePendingJob;
    }

    public async Task<ExecuteAllPendingJobsResponse> Handle(ExecuteAllPendingJobsRequest request, CancellationToken cancellationToken)
    {
        while (await GetNextPendingJob(cancellationToken) is not null and var pendingJob)
        {
            var executeJobRequest = new ExecutePendingJobRequest
            {
                PendingJob = pendingJob
            };

            await _executePendingJob.Send(executeJobRequest, cancellationToken);
        }

        return DefaultResponse();
    }

    private Task<PendingJob?> GetNextPendingJob(CancellationToken cancellationToken)
    {
        return _appDbContext.PendingJobs.DbSet.FirstOrDefaultAsync(x => x.PendingJobStatus == PendingJobStatus.Pending, cancellationToken);
    }

    ExecuteAllPendingJobsResponse DefaultResponse() => new();
}
