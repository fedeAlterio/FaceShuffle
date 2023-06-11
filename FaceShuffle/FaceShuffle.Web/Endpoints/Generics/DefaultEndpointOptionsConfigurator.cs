namespace FaceShuffle.Web.Endpoints.Generics;

public static class DefaultEndpointOptionsConfigurator
{
    public static RouteHandlerBuilder WithDefaultEndpointConfiguration(this RouteHandlerBuilder @this)
    {
        return @this.WithOpenApi();
    }
}
