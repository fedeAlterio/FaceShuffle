using FaceShuffle.Application.Abstractions;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.UpdateUserProfile;
public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileRequest, UpdateUserProfileResponse>
{
    private readonly IAppDbContext _appDbContext;

    public UpdateUserProfileHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<UpdateUserProfileResponse> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
    {
        var userSession = await _appDbContext.UserSessions.FindSessionByGuid(request.UserIdentity.UserSessionGuid, cancellationToken);
        userSession.UserAge = request.UserAge;
        userSession.Bio = request.Bio;
        userSession.UserFullName = request.UserFullName;

        return new();
    }
}
