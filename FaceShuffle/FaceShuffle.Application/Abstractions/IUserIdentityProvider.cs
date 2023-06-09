using FaceShuffle.Models;

namespace FaceShuffle.Application.Abstractions;
public interface IUserIdentityProvider
{
    public UserIdentity UserIdentity { get; set; }
}
