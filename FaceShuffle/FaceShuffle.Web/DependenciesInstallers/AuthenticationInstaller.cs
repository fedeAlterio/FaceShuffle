using System.Text;
using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Abstractions.Auth;
using FaceShuffle.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FaceShuffle.Web.Utilities;

namespace FaceShuffle.Web.DependenciesInstallers;

public static class AuthenticationInstaller
{
    public static void AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = configuration.Get<JwtConfiguration>("JWT");
        services.AddSingleton(jwtConfiguration);
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key))
            };
        });

        services.AddAuthorization();

        services.AddScoped<IUserIdentityProvider, UserIdentityProvider>();
        services.AddScoped(s => s.GetRequiredService<IUserIdentityProvider>().UserIdentity);
    }
}
