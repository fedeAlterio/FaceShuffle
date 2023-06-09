using FaceShuffle.Application.Abstractions;
using FaceShuffle.Models.Events;

namespace FaceShuffle.Infrastructure;
public class DomainEventsCollector : IDomainEventsCollector
{
    private readonly Queue<IDomainEvent> _domainEvents = new();

    public void AddNewEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Enqueue(domainEvent);
    }

#pragma warning disable CS8767
    public bool TryDequeueDomainEvent(out IDomainEvent? domainEvent)
#pragma warning restore CS8767
    {
        return _domainEvents.TryDequeue(out domainEvent);
    }
}
