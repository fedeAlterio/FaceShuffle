namespace FaceShuffle.Web.Endpoints.Utilities;

public static class DefaultEndpointOptionsConfigurator
{
    public static RouteHandlerBuilder WithDefaultConfiguration(this RouteHandlerBuilder @this)
    {
        return @this.WithOpenApi();
    }
}
