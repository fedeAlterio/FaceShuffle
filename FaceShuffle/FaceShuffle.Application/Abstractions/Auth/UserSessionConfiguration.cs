namespace FaceShuffle.Application.Abstractions.Auth;
public class UserSessionConfiguration
{
    public int MinutesBeforeExpiration { get; init; } = 10;
    public int ExpiredSessionsDeletionPollingPeriodMinutes { get; init; } = 10;
}
