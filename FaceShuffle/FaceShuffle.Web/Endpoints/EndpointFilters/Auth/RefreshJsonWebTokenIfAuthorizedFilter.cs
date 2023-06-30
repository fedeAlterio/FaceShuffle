using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Abstractions.Auth;
using FaceShuffle.Models.Generic;

namespace FaceShuffle.Web.Endpoints.EndpointFilters.Auth;

public class RefreshJsonWebTokenIfAuthorizedFilter : IEndpointFilter
{
    private readonly IAuthService _authService;
    private readonly IUserIdentityProvider _userIdentityProvider;
    private readonly IAppDbContext _appDbContext;

    public RefreshJsonWebTokenIfAuthorizedFilter(IAuthService authService, 
        IUserIdentityProvider userIdentityProvider,
        IAppDbContext appDbContext)
    {
        _authService = authService;
        _userIdentityProvider = userIdentityProvider;
        _appDbContext = appDbContext;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if(context.HttpContext.User.Identity?.IsAuthenticated is not true)
            return await NotAuthenticatedNext();

        var userIdentityOptional = _authService.UserIdentityFromPrincipalIdentity(context.HttpContext.User.Identity);
        if (!userIdentityOptional.TryGetValue(out var userIdentity))
            return await NotAuthenticatedNext();

        var userExists = await _appDbContext.UserSessions.ExistsUsername(userIdentity.Username, context.HttpContext.RequestAborted);
        if (!userExists)
            return Results.Unauthorized();

        _userIdentityProvider.UserIdentity = userIdentity;
        var ret = await next(context);
        var token = _authService.CreateJsonWebTokenFromUserIdentity(userIdentity);
        context.HttpContext.Response.Headers.Add("refreshed-jwt-token", token);

        return ret;

        ValueTask<object?> NotAuthenticatedNext() => next(context);
    }
}
