using FaceShuffle.Application.Abstractions;
using FaceShuffle.Web.Endpoints.Abstractions;

namespace FaceShuffle.Web.Endpoints.Session.AddUserPicture;

public class AddUserPictureEndpoint : IEndpoint
{
    public void MapEndpoint(RouteGroupBuilder endpoints)
    {
        endpoints
            .MapPost(@"/AddPicture", (IFormFile picture,
                IRequestSender<AddUserPictureWebRequest, AddUserPictureWebResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var request = new AddUserPictureWebRequest
                {
                    PictureFile = picture
                };

                return handler.Send(request, cancellationToken);
            })
            .WithName("Add picture")
            .RequireAuthorization();
    }
}
