using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FaceShuffle.Web.Endpoints.Utilities;

public static class EndpointHandlers
{
    public static Delegate From<TRequest, TResponse>() where TRequest : IRequest<TResponse>
    {
        return async ([FromBody] TRequest request, 
                      [FromServices] IRequestHandler<TRequest, TResponse> handler,
                      CancellationToken cancellationToken) =>
        {
            var response = await handler.Handle(request, cancellationToken);
            return response;
        };
    }
}
