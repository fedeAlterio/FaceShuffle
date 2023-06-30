using FaceShuffle.Models;
using FaceShuffle.Models.Session;
using System.Security.Principal;
using FaceShuffle.Models.Generic;

namespace FaceShuffle.Application.Abstractions.Auth;
public interface IAuthService
{
    string CreateJsonWebTokenFromUserIdentity(UserIdentity userSession);
    UserIdentity CreateUserIdentityFromUserSession(UserSession userSession);
    Optional<UserIdentity> UserIdentityFromPrincipalIdentity(IIdentity? claimsIdentity);
}
