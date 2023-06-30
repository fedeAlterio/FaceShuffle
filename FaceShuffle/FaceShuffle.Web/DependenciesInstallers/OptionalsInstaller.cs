using FaceShuffle.Application.Abstractions;
using FaceShuffle.Models.Generic;

namespace FaceShuffle.Web.DependenciesInstallers;

public static class OptionalsInstaller
{
    public static void AddOptionalDependencies(this IServiceCollection @this)
    {
        @this.AddTransient(typeof(IOptionalDependency<>), typeof(OptionalDependency<>));
    }

    class OptionalDependency<T> : IOptionalDependency<T>
    {
        public OptionalDependency(IServiceProvider serviceProvider)
        {
            Optional = serviceProvider.GetService<T>().ToOptional();
        }

        
        public Optional<T> Optional { get; }
    }
}
