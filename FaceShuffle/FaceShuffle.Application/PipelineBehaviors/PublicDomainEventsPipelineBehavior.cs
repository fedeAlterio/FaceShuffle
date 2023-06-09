using FaceShuffle.Application.Abstractions;
using MediatR;

namespace FaceShuffle.Application.PipelineBehaviors;

public sealed class PublishDomainEventsPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IDomainEventsCollector _domainEventCollector;
    private readonly IPublisher _publisher;

    public PublishDomainEventsPipelineBehavior(IDomainEventsCollector domainEventCollector, IPublisher publisher)
    {
        _domainEventCollector = domainEventCollector;
        _publisher = publisher;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        while (_domainEventCollector.TryDequeueDomainEvent(out var domainEvent))
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        return response;
    }
}
