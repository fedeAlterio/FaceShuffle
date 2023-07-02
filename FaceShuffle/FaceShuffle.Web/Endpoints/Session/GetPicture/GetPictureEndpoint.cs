using FaceShuffle.Application.Abstractions;
using FaceShuffle.Web.Endpoints.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FaceShuffle.Web.Endpoints.Session.GetPicture;
public class GetPictureEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder endpoints)
    {
        endpoints
            .MapGet(@"/Picture/{pictureGuid}", async
                (Guid pictureGuid,
                [FromServices] IRequestSender<GetPictureWebRequest, GetPictureWebResponse> sender,
                CancellationToken cancellationToken) =>
            {
                var request = new GetPictureWebRequest
                {
                    UserPictureGuid = pictureGuid
                };

                var response = await sender.Send(request, cancellationToken);
                return response.Result;
            })
            .WithName("Get Picture");
    }
}
