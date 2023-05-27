using FaceShuffle.Web.Endpoints.Abstractions;

namespace FaceShuffle.Web.Endpoints.Utilities;

public static class EndpointsInstaller
{
    public static void AddEndpoints(this IEndpointRouteBuilder endpointsBuilder)
    {
        var assembly = typeof(IAssemblyMarker).Assembly;
        var endpointConfigurators = from type in assembly.ExportedTypes
                                    where type is { IsAbstract: false, IsInterface: false }
                                          && typeof(IEndpoint).IsAssignableFrom(type)
                                    select (IEndpoint) ActivatorUtilities.CreateInstance(endpointsBuilder.ServiceProvider, type)!;

        foreach (var configurator in endpointConfigurators)
        {
            configurator.MapEndpoint(endpointsBuilder);
        }
    }
}
