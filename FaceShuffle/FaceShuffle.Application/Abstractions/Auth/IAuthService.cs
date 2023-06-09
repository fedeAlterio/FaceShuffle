using FaceShuffle.Models;
using System.Security.Principal;

namespace FaceShuffle.Application.Abstractions.Auth;
public interface IAuthService
{
    string CreateJsonWebTokenFromUserIdentity(UserIdentity userSession);
    UserIdentity CreateUserIdentityFromUserSession(UserSession userSession);
    UserIdentity UserIdentityFromPrincipalIdentity(IIdentity? claimsIdentity);
}
