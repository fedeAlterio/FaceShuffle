using System.Text;
using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Abstractions.Auth;
using FaceShuffle.Infrastructure.Auth;
using FaceShuffle.Web.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FaceShuffle.Web.Utilities;

namespace FaceShuffle.Web.DependenciesInstallers;

public static class AuthenticationInstaller
{
    public static void AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var userSessionConfigurationRaw = configuration.GetSection(ConfigurationSections.UserSession);
        var jwtConfigurationRaw = configuration.GetSection(ConfigurationSections.Jwt);

        var jwtConfiguration = configuration.Get<JwtConfiguration>(ConfigurationSections.Jwt);
        services.Configure<JwtConfiguration>(jwtConfigurationRaw);
        services.Configure<UserSessionConfiguration>(userSessionConfigurationRaw);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfiguration.Issuer,
                ValidAudience = jwtConfiguration.Audience,
                ClockSkew = TimeSpan.FromSeconds(jwtConfiguration.ClockSkewSeconds),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key))
            };
        });

        services.AddAuthorization();

        services.AddScoped<IUserIdentityProvider, UserIdentityProvider>();
        services.AddScoped(s => s.GetRequiredService<IUserIdentityProvider>().UserIdentity);
    }
}
