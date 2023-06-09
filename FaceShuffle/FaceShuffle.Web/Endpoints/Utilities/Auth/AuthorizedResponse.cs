namespace FaceShuffle.Web.Endpoints.Utilities.Auth;

public record AuthorizedResponse<T>
{
    public required string Token { get; init; }
    public required T Data { get; init; }
}
