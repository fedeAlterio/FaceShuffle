using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace FaceShuffle.Web.Endpoints.Generics.Auth;

public static class AuthorizedMediatorEndpoint
{
    public static async Task<AuthorizedWebResponse<TResponse>> Handle<TRequest, TResponse>(
        [FromBody] TRequest request,
        ClaimsPrincipal claims,
        AuthorizedWebRequestHandler<TRequest, TResponse> authorizedWebRequestHandler,
        CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
    {
        var authorizedRequest = new AuthorizedWebRequest<TRequest, TResponse>
        {
            ClaimsPrincipal = claims,
            Request = request
        };

        var authorizedResponse = await authorizedWebRequestHandler.Handle(authorizedRequest, cancellationToken);
        return authorizedResponse;
    }
}
