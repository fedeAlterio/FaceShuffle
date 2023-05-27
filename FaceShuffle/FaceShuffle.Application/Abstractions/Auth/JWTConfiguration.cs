namespace FaceShuffle.Application.Abstractions.Auth;

public class JwtConfiguration
{
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string Key { get; init; } = null!;
    public int MinutesBeforeExpiration { get; init; } 
}
