using FaceShuffle.Application.Exceptions;

namespace FaceShuffle.Web.Endpoints.EndpointFilters.ExceptionsFilters;

public class UserReadableAppExceptionFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (UserReadableAppException e)
        {
            return Results.Problem(e.UserText);
        }
    }
}
