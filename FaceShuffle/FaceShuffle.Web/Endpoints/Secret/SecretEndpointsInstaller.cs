namespace FaceShuffle.Web.Endpoints.Secret;

internal static class SecretEndpointsInstaller
{
    public static RouteGroupBuilder MapSecretsEndpoints(this RouteGroupBuilder @this, IServiceProvider serviceProvider)
    {
        var secretEndpoints = @this.MapGroup("Secret");
        secretEndpoints
            .RequireAuthorization()
            .MapEndpoint<SecretEndpoint>(serviceProvider);

        return secretEndpoints;
    }
}
