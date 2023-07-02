using MediatR;

namespace FaceShuffle.Web.Endpoints.Session.GetUserProfile;
public class GetUserProfileWebRequest : IRequest<GetUserProfileWebResponse>
{
    public string Username { get; init; } = null!;
}
