using FaceShuffle.Models.Session;


namespace FaceShuffle.Application.Actions.Session.GetUserProfile;
public class GetUserProfileResponse
{
    public required UserSession UserSession { get; init; }
}
