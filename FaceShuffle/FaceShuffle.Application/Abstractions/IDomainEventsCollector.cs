using System.Diagnostics.CodeAnalysis;
using FaceShuffle.Models.Events;

namespace FaceShuffle.Application.Abstractions;
public interface IDomainEventsCollector
{
    void AddNewEvent(IDomainEvent domainEvent);
    bool TryDequeueDomainEvent([NotNullWhen(true)] out IDomainEvent? domainEvent);
}
