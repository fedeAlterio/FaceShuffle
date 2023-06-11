using FaceShuffle.Application.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FaceShuffle.Web.Endpoints.Generics;

public static class MediatorEndpoint
{
    public static async Task<TResponse> Handle<TRequest, TResponse>(
        [FromBody] TRequest request,
        IRequestSender<TRequest, TResponse> handler,
        CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
    {
        var response = await handler.Send(request, cancellationToken);
        return response;
    }
}
