using FaceShuffle.Models.Session;
using FaceShuffle.Models.UserPictures;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.GetPicture;
public class GetPictureRequest : IRequest<GetPictureResponse>
{
    public required UserSessionGuid UserSessionGuid { get; init; }
    public required UserPictureGuid UserPictureGuid { get; init; }
}
