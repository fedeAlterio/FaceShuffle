using FaceShuffle.Models.Events;

namespace FaceShuffle.Models.PendingJobs.Events;

public record PendingJobAddedEvent(PendingJob PendingJob) : IDomainEvent;
