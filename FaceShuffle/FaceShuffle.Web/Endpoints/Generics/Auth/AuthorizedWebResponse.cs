namespace FaceShuffle.Web.Endpoints.Generics.Auth;

public record AuthorizedWebResponse<T>
{
    public required string Token { get; init; }
    public required T Data { get; init; }
}
