namespace FaceShuffle.Models.Events;

public class UserAuthenticatedEvent : IDomainEvent
{
    public required UserIdentity UserIdentity { get; init; }
}
