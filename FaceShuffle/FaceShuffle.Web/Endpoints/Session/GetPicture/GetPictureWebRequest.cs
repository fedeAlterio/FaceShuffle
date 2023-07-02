using MediatR;

namespace FaceShuffle.Web.Endpoints.Session.GetPicture;
public class GetPictureWebRequest : IRequest<GetPictureWebResponse>
{
    public required Guid UserPictureGuid { get; init; }
}
