using MediatR;

namespace FaceShuffle.Web.Endpoints.Session.UpdateUserProfile;
public class UpdateUserProfileWebRequest : IRequest<UpdateUserProfileWebResponse>
{
    public int UserAge { get; init; }
    public string Bio { get; init; } = null!;
    public string UserFullName { get; init; } = null!;
}
