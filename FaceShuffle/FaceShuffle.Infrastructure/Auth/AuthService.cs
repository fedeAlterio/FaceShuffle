using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using FaceShuffle.Application.Abstractions.Auth;
using FaceShuffle.Models;
using Microsoft.IdentityModel.Tokens;

namespace FaceShuffle.Infrastructure.Auth;
public class AuthService : IAuthService
{
    private readonly JwtConfiguration _configuration;

    public AuthService(JwtConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateJsonWebTokenFromUserIdentity(UserIdentity userIdentity)
    {
        var claims = ClaimsFromUserIdentity(userIdentity);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(_configuration.Issuer, _configuration.Audience, claims,
            expires: DateTime.Now.AddMinutes(_configuration.MinutesBeforeExpiration), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public UserIdentity CreateUserIdentityFromUserSession(UserSession userSession)
    {
        return new()
        {
            Name = userSession.Name,
        };
    }


    public IEnumerable<Claim> ClaimsFromUserIdentity(UserIdentity userIdentity)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userIdentity.Name),
        };

        return claims;
    }

    public UserIdentity UserIdentityFromPrincipalIdentity(IIdentity? identity)
    {
        var claimsIdentity = identity as ClaimsIdentity
                             ?? throw new InvalidOperationException(
                                 $"Can't extract user identity: Expected claims identity but found [{identity?.GetType() }]");

        var name = claimsIdentity.FindFirst(ClaimTypes.Name)!.Value;

        return new()
        {
            Name = name
        };
    }
}
