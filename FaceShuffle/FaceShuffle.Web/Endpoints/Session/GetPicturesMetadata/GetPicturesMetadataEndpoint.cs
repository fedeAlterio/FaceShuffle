using FaceShuffle.Web.Endpoints.Abstractions;
using FaceShuffle.Web.Endpoints.Generics;

namespace FaceShuffle.Web.Endpoints.Session.GetPicturesMetadata;
public class GetPicturesMetadataEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder endpoints)
    {
        endpoints
            .MapGet(@"/GetPicturesMetadata",
                MediatorEndpoint.FromEmptyRequest<GetPicturesMetadataWebRequest, GetPicturesMetadataWebResponse>)
            .WithName("Get pictures metadata")
            .RequireAuthorization();
    }
}
