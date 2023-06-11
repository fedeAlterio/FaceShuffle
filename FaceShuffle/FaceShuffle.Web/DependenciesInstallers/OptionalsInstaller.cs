using FaceShuffle.Application.Abstractions;
using Optional;

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
            Optional = serviceProvider.GetService<T>().SomeNotNull()!;
        }

        
        public Option<T> Optional { get; }
    }
}
