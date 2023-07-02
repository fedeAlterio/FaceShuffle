using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.Session.GetUserProfile;
using MediatR;

namespace FaceShuffle.Web.Endpoints.Session.GetUserProfile;
public class GetUserProfileWebHandler : IRequestHandler<GetUserProfileWebRequest, GetUserProfileWebResponse>
{
    private readonly IRequestSender<GetUserProfileRequest, GetUserProfileResponse> _requestSender;

    public GetUserProfileWebHandler(IRequestSender<GetUserProfileRequest, GetUserProfileResponse> requestSender)
    {
        _requestSender = requestSender;
    }

    public async Task<GetUserProfileWebResponse> Handle(GetUserProfileWebRequest webRequest, CancellationToken cancellationToken)
    {
        var request = new GetUserProfileRequest
        {
            Username = new(webRequest.Username)
        };

        var response = await _requestSender.Send(request, cancellationToken);
        var session = response.UserSession;

        return new()
        {
            SessionGuid = session.SessionGuid.Value,
            Bio = session.Bio.Value,
            CreationDate = session.CreationDate,
            LastSeenDate = session.LastSeenDate,
            UserAge = session.UserAge.Value,
            UserFullName = session.UserFullName.Value,
            Username = session.Username.Value,
        };
    }
}
