using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.PendingJobs;
using FaceShuffle.Application.Actions.PendingJobs.PendingJobEnqueuers;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.InvalidateSession;
public class InvalidateSessionHandler : IRequestHandler<InvalidateSessionRequest, InvalidateSessionResponse>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IPendingJobService _pendingJobService;

    public InvalidateSessionHandler(
        IAppDbContext appDbContext,
        IPendingJobService pendingJobService)
    {
        _appDbContext = appDbContext;
        _pendingJobService = pendingJobService;
    }

    public async Task<InvalidateSessionResponse> Handle(InvalidateSessionRequest request, CancellationToken cancellationToken)
    {
        var userSessions = _appDbContext.UserSessions;

        var userSession = await userSessions.FindSessionById(request.UserSessionId, cancellationToken);
        userSessions.DbSet.Remove(userSession);
        
        await _pendingJobService.EnqueueNewDeleteUserPicturesJob(new(request.UserSessionGuid), cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return new();
    }
}
