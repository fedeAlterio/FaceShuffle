using FaceShuffle.Models;
using FaceShuffle.Models.Session;
using System.Security.Principal;

namespace FaceShuffle.Application.Abstractions.Auth;
public interface IAuthService
{
    string CreateJsonWebTokenFromUserIdentity(UserIdentity userSession);
    UserIdentity CreateUserIdentityFromUserSession(UserSession userSession);
    UserIdentity UserIdentityFromPrincipalIdentity(IIdentity? claimsIdentity);
}
