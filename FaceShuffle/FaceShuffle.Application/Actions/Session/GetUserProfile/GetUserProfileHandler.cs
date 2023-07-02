using FaceShuffle.Application.Repositories;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.GetUserProfile;
public class GetUserProfileHandler : IRequestHandler<GetUserProfileRequest, GetUserProfileResponse>
{
    private readonly IUserSessionRepository _userSessionRepository;

    public GetUserProfileHandler(IUserSessionRepository userSessionRepository)
    {
        _userSessionRepository = userSessionRepository;
    }

    public async Task<GetUserProfileResponse> Handle(GetUserProfileRequest request, CancellationToken cancellationToken)
    {
        var session = await _userSessionRepository.FindSessionByUsername(request.Username, cancellationToken);

        return new()
        {
            UserSession = session
        };
    }
}
