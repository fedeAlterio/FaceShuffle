namespace FaceShuffle.Web.Endpoints.Abstractions;

internal interface IEndpoint
{
    void MapEndpoint(RouteGroupBuilder routeGroupBuilder);
}   