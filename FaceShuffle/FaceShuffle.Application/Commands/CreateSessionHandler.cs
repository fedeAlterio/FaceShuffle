using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Abstractions.Auth;
using FaceShuffle.Models;
using MediatR;

namespace FaceShuffle.Application.Commands;
internal class CreateSessionHandler : IRequestHandler<CreateSessionRequest, CreateSessionResponse>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IAuthService _authService;

    public CreateSessionHandler(IAppDbContext appDbContext, IAuthService authService)
    {
        _appDbContext = appDbContext;
        _authService = authService;
    }

    public async Task<CreateSessionResponse> Handle(CreateSessionRequest request, CancellationToken cancellationToken)
    {
        var userSession = new UserSession
        {
            Name = request.Name,
            CreationDate = DateTime.UtcNow,
            SessionGuid = Guid.NewGuid()
        };

        var createdUserSessionEntity = await _appDbContext.UserSessions.DbSet.AddAsync(userSession, cancellationToken);
        var createdUserSession = createdUserSessionEntity.Entity;

        var token = _authService.CreateJsonWebTokenFromUserSession(createdUserSession);

        var ret = new CreateSessionResponse
        {
            UserSession = createdUserSession,
            JsonWebToken = token
        };

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return ret;
    }
}
