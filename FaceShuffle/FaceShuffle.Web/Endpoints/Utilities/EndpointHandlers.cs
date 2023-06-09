using System.Security.Claims;
using FaceShuffle.Web.Endpoints.Utilities.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FaceShuffle.Web.Endpoints.Utilities;

public static class EndpointHandlers
{
    public delegate Task<TResponse> MediatorEndpointHandler<TRequest, TResponse>(TRequest request,
                                                                                 IRequestHandler<TRequest, TResponse> handler,
                                                                                 CancellationToken cancellationToken)
                                                                where TRequest : IRequest<TResponse>;

    public delegate Task<AuthorizedResponse<TResponse>> AuthorizedMediatorEndpointHandler<TRequest, TResponse>(
        TRequest request,
        ClaimsPrincipal claims,
        AuthorizedRequestHandler<TRequest, TResponse> authorizedRequestHandler,
        CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>;


    public static AuthorizedMediatorEndpointHandler<TRequest, TResponse> AuthorizedFrom<TRequest, TResponse>() where TRequest : IRequest<TResponse>
    {
        return async (
            [FromBody] request,
            claims,
            authorizedRequestHandler,
            cancellationToken) =>
        {
            var authorizedRequest = new AuthorizedRequest<TRequest, TResponse>
            {
                ClaimsPrincipal = claims,
                Request = request
            };

            var authorizedResponse = await authorizedRequestHandler.Handle(authorizedRequest, cancellationToken);
            return authorizedResponse;
        };
    }

    public static MediatorEndpointHandler<TRequest, TResponse> From<TRequest, TResponse>() where TRequest : IRequest<TResponse>
    {
        return async ([FromBody] request, handler, cancellationToken) =>
        {
            var response = await handler.Handle(request, cancellationToken);
            return response;
        };
    }
}
