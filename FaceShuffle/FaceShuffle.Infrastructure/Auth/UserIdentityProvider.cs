using FaceShuffle.Application.Abstractions;
using FaceShuffle.Models;
using FaceShuffle.Models.Events;
using FaceShuffle.Models.Exceptions;

namespace FaceShuffle.Infrastructure.Auth;

public class UserIdentityProvider : IUserIdentityProvider
{
    private readonly IDomainEventsCollector _domainEventsCollector;

    public UserIdentityProvider(IDomainEventsCollector domainEventsCollector)
    {
        _domainEventsCollector = domainEventsCollector;
    }

    private UserIdentity? _userIdentity;
    public UserIdentity UserIdentity
    {
        get => _userIdentity ?? throw new ExceptionBase("Authentication is needed!");
        set
        {
            if (EqualityComparer<UserIdentity>.Default.Equals(_userIdentity, value))
                return;

            _userIdentity = value;
            var userAuthenticatedEvent = new UserAuthenticatedEvent
            {
                UserIdentity = value
            };

            _domainEventsCollector.AddNewEvent(userAuthenticatedEvent);
        } 
    }
}
