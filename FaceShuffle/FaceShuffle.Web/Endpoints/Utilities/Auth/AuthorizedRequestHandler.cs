using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Abstractions.Auth;
using MediatR;

namespace FaceShuffle.Web.Endpoints.Utilities.Auth;

public class AuthorizedRequestHandler<TRequest, TResponse> : IRequestHandler<AuthorizedRequest<TRequest, TResponse>, AuthorizedResponse<TResponse>> 
    where TRequest : IRequest<TResponse>
{
    private readonly IAuthService _authService;
    private readonly IUserIdentityProvider _userIdentityProvider;
    private readonly Lazy<IRequestHandler<TRequest, TResponse>> _handler;

    public AuthorizedRequestHandler(
        IAuthService authService, 
        IUserIdentityProvider userIdentityProvider,
        Lazy<IRequestHandler<TRequest, TResponse>> handler)
    {
        _authService = authService;
        _userIdentityProvider = userIdentityProvider;
        _handler = handler;
    }

    public async Task<AuthorizedResponse<TResponse>> Handle(AuthorizedRequest<TRequest, TResponse> authorizedRequest, CancellationToken cancellationToken)
    {
        var userIdentity = _authService.UserIdentityFromPrincipalIdentity(authorizedRequest.ClaimsPrincipal.Identity);
        _userIdentityProvider.UserIdentity = userIdentity;
        var response = await _handler.Value.Handle(authorizedRequest.Request, cancellationToken);
        var token = _authService.CreateJsonWebTokenFromUserIdentity(userIdentity);

        return new()
        {
            Token = token,
            Data = response
        };
    }
}
