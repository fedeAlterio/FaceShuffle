using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using FaceShuffle.Application.Abstractions.Auth;
using FaceShuffle.Models;
using FaceShuffle.Models.Generic;
using FaceShuffle.Models.Session;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FaceShuffle.Infrastructure.Auth;
public class AuthService : IAuthService
{
    private readonly IOptions<JwtConfiguration> _jwtConfiguration;
    private readonly IOptions<UserSessionConfiguration> _userSessionConfiguration;
    private const string ClaimUsername = "ClaimUsername";
    private const string ClaimSessionGuid = "ClaimSessionGuid";

    public AuthService(IOptions<JwtConfiguration> jwtConfiguration, IOptions<UserSessionConfiguration> userSessionConfiguration)
    {
        _jwtConfiguration = jwtConfiguration;
        _userSessionConfiguration = userSessionConfiguration;
    }

    public string CreateJsonWebTokenFromUserIdentity(UserIdentity userIdentity)
    {
        var claims = ClaimsFromUserIdentity(userIdentity);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Value.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(_jwtConfiguration.Value.Issuer, _jwtConfiguration.Value.Audience, claims,
            expires: DateTime.Now.AddMinutes(_userSessionConfiguration.Value.MinutesBeforeExpiration), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public UserIdentity CreateUserIdentityFromUserSession(UserSession userSession)
    {
        return new()
        {
            Username = userSession.Username,
            UserSessionGuid = userSession.SessionGuid,
        };
    }


    public IEnumerable<Claim> ClaimsFromUserIdentity(UserIdentity userIdentity)
    {
        var claims = new[]
        {
            new Claim(ClaimUsername, userIdentity.Username.Value),
            new Claim(ClaimSessionGuid, userIdentity.UserSessionGuid.Value.ToString())
        };

        return claims;
    }

    public Optional<UserIdentity> UserIdentityFromPrincipalIdentity(IIdentity? identity)
    {
        if (identity is not ClaimsIdentity claimsIdentity)
            return default;

        var username = claimsIdentity.FindFirst(ClaimUsername)?.Value;
        if(username is null)
            return default;

        var sessionGuid = claimsIdentity.FindFirst(ClaimSessionGuid)?.Value;
        if(sessionGuid is null)
            return default;

        return new UserIdentity
        {
            Username = new(username),
            UserSessionGuid = new(Guid.Parse(sessionGuid))
        };
    }
}
