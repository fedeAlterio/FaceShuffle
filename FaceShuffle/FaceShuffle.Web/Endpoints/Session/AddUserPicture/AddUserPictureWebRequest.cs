using MediatR;

namespace FaceShuffle.Web.Endpoints.Session.AddUserPicture;

public class AddUserPictureWebRequest : IRequest<AddUserPictureWebResponse>
{
    public IFormFile PictureFile { get; init; } = null!;
}
