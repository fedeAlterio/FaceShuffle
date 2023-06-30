using MediatR;

namespace FaceShuffle.Web.Endpoints.Session.AddUserPicture;

public class AddUserPictureWebRequest : IRequest<AddUserPictureWebResponse>
{
    public required IFormFile PictureFile { get; init; }
}
