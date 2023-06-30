using FaceShuffle.Web.Endpoints.Abstractions;
using FaceShuffle.Web.Endpoints.Generics;

namespace FaceShuffle.Web.Endpoints.Session.GetPicturesMetadata;
public class GetPicturesMetadataEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder endpoints)
    {
        endpoints
            .MapPost(@"/GetPicturesMetadata",
                MediatorEndpoint.FromBody<GetPicturesMetadataWebRequest, GetPicturesMetadataWebResponse>)
            .WithName("GetPicturesMetadata")
            .RequireAuthorization();
    }
}
