using System.Security.Claims;
using MediatR;

namespace FaceShuffle.Web.Endpoints.Utilities.Auth;

public class AuthorizedRequest<TRequest, TResponse> : IRequest<AuthorizedResponse<TResponse>> where TRequest : IRequest<TResponse>
{
    public required ClaimsPrincipal ClaimsPrincipal { get; init; }
    public required TRequest Request { get; init; }
}
