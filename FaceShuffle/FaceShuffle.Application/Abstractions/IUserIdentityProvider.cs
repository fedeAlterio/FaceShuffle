using FaceShuffle.Models;

namespace FaceShuffle.Application.Abstractions;
public interface IUserIdentityProvider
{
    UserIdentity UserIdentity { get; set; }
}
