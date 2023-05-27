using FaceShuffle.Models;
using System.Security.Principal;

namespace FaceShuffle.Application.Abstractions.Auth;
public interface IAuthService
{
    string CreateJsonWebTokenFromUserSession(UserSession userSession);
    UserIdentity GetUserIdentity(IIdentity? claimsIdentity);
}
