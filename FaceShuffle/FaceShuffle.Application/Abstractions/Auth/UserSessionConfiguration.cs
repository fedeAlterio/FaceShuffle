namespace FaceShuffle.Application.Abstractions.Auth;
public class UserSessionConfiguration
{
    public int MinutesBeforeExpiration { get; init; }
    public int ExpiredSessionsDeletionPollingPeriodMinutes { get; init; }
}
