using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Abstractions.Auth;
using MediatR;

namespace FaceShuffle.Web.Endpoints.Generics.Auth;

public class AuthorizedWebRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IAuthService _authService;
    private readonly IUserIdentityProvider _userIdentityProvider;
    private readonly IRequestSender<TRequest, TResponse> _handler;

    public AuthorizedWebRequestHandler(
        IAuthService authService, 
        IUserIdentityProvider userIdentityProvider,
        IRequestSender<TRequest, TResponse> handler)
    {
        _authService = authService;
        _userIdentityProvider = userIdentityProvider;
        _handler = handler;
    }

    public async Task<AuthorizedWebResponse<TResponse>> Handle(AuthorizedWebRequest<TRequest, TResponse> authorizedWebRequest, CancellationToken cancellationToken)
    {
        var userIdentity = _authService.UserIdentityFromPrincipalIdentity(authorizedWebRequest.ClaimsPrincipal.Identity);
        _userIdentityProvider.UserIdentity = userIdentity;
        var response = await _handler.Send(authorizedWebRequest.Request, cancellationToken);
        var token = _authService.CreateJsonWebTokenFromUserIdentity(userIdentity);

        return new()
        {
            Token = token,
            Data = response
        };
    }
}
