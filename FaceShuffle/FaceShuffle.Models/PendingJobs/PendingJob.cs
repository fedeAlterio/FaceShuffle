namespace FaceShuffle.Models.PendingJobs;
public class PendingJob
{
    public int Id { get; set; }
    public required PendingJobType Type { get; init; }
    public required string Payload { get; init; }
}
