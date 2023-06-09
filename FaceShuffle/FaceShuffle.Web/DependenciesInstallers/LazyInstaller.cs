namespace FaceShuffle.Web.DependenciesInstallers;

public static class LazyInstaller
{
    public static void AddLazyServices(this IServiceCollection @this)
    {
        @this.AddTransient(typeof(Lazy<>), typeof(Lazier<>));
    }
    
    class Lazier<T> : Lazy<T> where T : class
    {
        public Lazier(IServiceProvider provider)
            : base(provider.GetRequiredService<T>)
        {
        }
    }
}
