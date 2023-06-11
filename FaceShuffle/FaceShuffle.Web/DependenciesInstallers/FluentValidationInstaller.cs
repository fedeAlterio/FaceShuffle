using FluentValidation;

namespace FaceShuffle.Web.DependenciesInstallers;

public static class FluentValidationInstaller
{
    public static void AddFluentValidation(this IServiceCollection @this)
    {
        @this.AddValidatorsFromAssemblyContaining(typeof(IAssemblyMarker));
        @this.AddValidatorsFromAssemblyContaining(typeof(Application.IAssemblyMarker));
    }
}
