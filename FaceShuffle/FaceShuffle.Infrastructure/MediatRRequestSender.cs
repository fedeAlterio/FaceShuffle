using FaceShuffle.Application.Abstractions;
using MediatR;

namespace FaceShuffle.Infrastructure;
public class MediatRRequestSender<TRequest, TResponse> : IRequestSender<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IMediator _mediator;

    public MediatRRequestSender(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResponse> Send(TRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(request, cancellationToken);
    }
}
