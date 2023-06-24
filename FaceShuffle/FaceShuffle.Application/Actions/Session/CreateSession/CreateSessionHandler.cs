using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Abstractions.Auth;
using FaceShuffle.Application.Extensions;
using FaceShuffle.Models.Session;
using MediatR;
using Microsoft.Extensions.Options;

namespace FaceShuffle.Application.Actions.Session.CreateSession;
internal class CreateSessionHandler : IRequestHandler<CreateSessionRequest, CreateSessionResponse>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IAuthService _authService;
    private readonly UserSessionConfiguration _userSessionConfiguration;

    public CreateSessionHandler(
        IAppDbContext appDbContext, 
        IAuthService authService,
        IOptionsMonitor<UserSessionConfiguration> userSessionConfiguration
        )
    {
        _appDbContext = appDbContext;
        _authService = authService;
        _userSessionConfiguration = userSessionConfiguration.CurrentValue;
    }

    public async Task<CreateSessionResponse> Handle(CreateSessionRequest request, CancellationToken cancellationToken)
    {
        var creationDate = DateTime.UtcNow;

        var userSession = new UserSession
        {
            SessionGuid = Guid.NewGuid(),
            CreationDate = creationDate,
            Username = request.Username,
            LastSeenDate = creationDate,
            MinutesBeforeExpiration = _userSessionConfiguration.MinutesBeforeExpiration
        };

        await _appDbContext.UserSessions.ThrowIfUsernameExists(request.Username, cancellationToken);
        
        var createdUserSessionEntity = await _appDbContext.UserSessions.DbSet.AddAsync(userSession, cancellationToken);
        var createdUserSession = createdUserSessionEntity.Entity;

        var identity = _authService.CreateUserIdentityFromUserSession(createdUserSession);
        var token = _authService.CreateJsonWebTokenFromUserIdentity(identity);

        var ret = new CreateSessionResponse
        {
            UserSession = createdUserSession,
            JsonWebToken = token
        };

        return ret;
    }
}
