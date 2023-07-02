using FaceShuffle.Models.Session;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.GetUserProfile;
public class GetUserProfileRequest : IRequest<GetUserProfileResponse>
{
    public required Username Username { get; init; }
}
