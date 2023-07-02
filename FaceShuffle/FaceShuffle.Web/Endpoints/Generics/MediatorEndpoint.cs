using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FaceShuffle.Web.Endpoints.Generics;

public static class MediatorEndpoint
{
    public static async Task<TResponse> FromBody<TRequest, TResponse>(
        [FromBody] TRequest request,
        IRequestSender<TRequest, TResponse> handler,
        CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
    {
        if (request is null)
            throw new UserReadableAppException { UserText = "A body is required" };

        var response = await handler.Send(request, cancellationToken);
        return response;
    }

    public static async Task<TResponse> FromEmptyRequest<TRequest, TResponse>(
        IRequestSender<TRequest, TResponse> handler,
        CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>, new()
    {
        TRequest request = new();
        var response = await handler.Send(request, cancellationToken);
        return response;
    }
}
