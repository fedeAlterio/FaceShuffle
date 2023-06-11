using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Abstractions.Auth;
using FaceShuffle.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var creationDate = DateTime.UtcNow;
        var sameNameCount = await _appDbContext.UserSessions.DbSet
            .Where(x => x.Name == request.Name)
            .CountAsync(cancellationToken);

        var username = $"{request.Name}_{sameNameCount + 1}";

        var userSession = new UserSession
        {
            Name = request.Name,
            CreationDate = creationDate,
            Username = username,
            LastSeenDate = creationDate
        };

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
