using FaceShuffle.Models.PendingJobs;
using MediatR;

namespace FaceShuffle.Application.Actions.PendingJobs.ExecutePendingJob;
public class ExecutePendingJobRequest : IRequest<ExecutePendingJobResponse>
{
    public required PendingJob PendingJob { get; init; }
}
