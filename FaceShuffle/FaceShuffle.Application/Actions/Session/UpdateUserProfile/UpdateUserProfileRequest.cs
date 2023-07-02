using FaceShuffle.Models;
using FaceShuffle.Models.Session;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.UpdateUserProfile;
public class UpdateUserProfileRequest : IRequest<UpdateUserProfileResponse>
{
    public required UserIdentity UserIdentity { get; init; }
    public required UserAge UserAge { get; set; }
    public required Bio Bio { get; set; }
    public required UserFullName UserFullName { get; set; }
}
