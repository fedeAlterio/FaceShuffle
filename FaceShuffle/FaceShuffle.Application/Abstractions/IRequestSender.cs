using MediatR;

namespace FaceShuffle.Application.Abstractions;
public interface IRequestSender<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Send(TRequest request, CancellationToken cancellationToken = default);
}
