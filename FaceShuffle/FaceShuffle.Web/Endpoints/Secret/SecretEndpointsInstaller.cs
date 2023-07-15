namespace FaceShuffle.Web.Endpoints.Secret;

internal static class SecretEndpointsInstaller
{
    public static RouteGroupBuilder MapSecretsEndpoints(this RouteGroupBuilder @this, IServiceProvider serviceProvider)
    {
        @this.MapGroup("Secret")
            .RequireAuthorization()
            .MapEndpoint<SecretEndpoint>(serviceProvider);

        return @this;
    }
}
