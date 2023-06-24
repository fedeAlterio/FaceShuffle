using FaceShuffle.Application.Abstractions;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.InvalidateSession;
public class InvalidateSessionHandler : IRequestHandler<InvalidateSessionRequest, InvalidateSessionResponse>
{
    private readonly IAppDbContext _appDbContext;

    public InvalidateSessionHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<InvalidateSessionResponse> Handle(InvalidateSessionRequest request, CancellationToken cancellationToken)
    {
        var userSessions = _appDbContext.UserSessions;
        var userSession = await userSessions.FindSessionById(request.SessionId, cancellationToken);
        userSessions.DbSet.Remove(userSession);
        return new();
    }
}
