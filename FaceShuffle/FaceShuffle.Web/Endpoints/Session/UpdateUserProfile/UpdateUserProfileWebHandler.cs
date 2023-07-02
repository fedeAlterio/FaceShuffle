using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.Session.UpdateUserProfile;
using FaceShuffle.Models;
using MediatR;

namespace FaceShuffle.Web.Endpoints.Session.UpdateUserProfile;
public class UpdateUserProfileWebHandler : IRequestHandler<UpdateUserProfileWebRequest, UpdateUserProfileWebResponse>
{
    private readonly UserIdentity _userIdentity;
    private readonly IRequestSender<UpdateUserProfileRequest, UpdateUserProfileResponse> _requestSender;

    public UpdateUserProfileWebHandler(UserIdentity userIdentity, IRequestSender<UpdateUserProfileRequest, UpdateUserProfileResponse> requestSender)
    {
        _userIdentity = userIdentity;
        _requestSender = requestSender;
    }

    public async Task<UpdateUserProfileWebResponse> Handle(UpdateUserProfileWebRequest webRequest, CancellationToken cancellationToken)
    {
        var request = new UpdateUserProfileRequest
        {
            Bio = new(webRequest.Bio),
            UserAge = new(webRequest.UserAge),
            UserFullName = new(webRequest.UserFullName),
            UserIdentity = _userIdentity
        };

        await _requestSender.Send(request, cancellationToken);

        return new();
    }
}
