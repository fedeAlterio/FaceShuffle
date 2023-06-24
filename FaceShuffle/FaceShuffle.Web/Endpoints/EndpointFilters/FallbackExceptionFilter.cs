namespace FaceShuffle.Web.Endpoints.EndpointFilters;

public class FallbackExceptionFilter : IEndpointFilter
{
    private readonly IWebHostEnvironment _hostEnvironment;

    public FallbackExceptionFilter(IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch
        {
            if (_hostEnvironment.IsDevelopment())
            {
                throw;
            }

            return Results.Problem();
        }
    }
}
