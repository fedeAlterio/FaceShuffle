using System.Security.Claims;
using MediatR;

namespace FaceShuffle.Web.Endpoints.Generics.Auth;

public class AuthorizedWebRequest<TRequest, TResponse> : IRequest<AuthorizedWebResponse<TResponse>> where TRequest : IRequest<TResponse>
{
    public required ClaimsPrincipal ClaimsPrincipal { get; init; }
    public required TRequest Request { get; init; }
}
