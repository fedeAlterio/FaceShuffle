using FaceShuffle.Web.Endpoints.Abstractions;
using FaceShuffle.Web.Endpoints.EndpointFilters.Auth;
using FaceShuffle.Web.Endpoints.EndpointFilters.ExceptionsFilters;
using FaceShuffle.Web.Endpoints.Messages;
using FaceShuffle.Web.Endpoints.Secret;
using FaceShuffle.Web.Endpoints.Session;

namespace FaceShuffle.Web.Endpoints;

internal static class EndpointsInstaller
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpointsBuilder)
    {
        var rootGroup = endpointsBuilder
            .MapGroup("")
            .AddEndpointFilter<FallbackExceptionFilter>()
            .AddEndpointFilter<FluentValidationExceptionFilter>()
            .AddEndpointFilter<UserReadableAppExceptionFilter>()
            .AddEndpointFilter<RefreshJsonWebTokenIfAuthorizedFilter>()
            .WithOpenApi();

        var serviceProvider = endpointsBuilder.ServiceProvider;

        rootGroup
            .MapSessionEndpoints(serviceProvider)
            .MapMessagesEndpoints(serviceProvider)
            .MapSecretsEndpoints(serviceProvider)
            ;
    }

    public static RouteGroupBuilder MapEndpoint<TEndpoint>(this RouteGroupBuilder @this, IServiceProvider serviceProvider) where TEndpoint : IEndpoint
    {
        var endpoint = ActivatorUtilities.CreateInstance<TEndpoint>(serviceProvider);
        endpoint.MapEndpoint(@this);
        return @this;
    }
}
