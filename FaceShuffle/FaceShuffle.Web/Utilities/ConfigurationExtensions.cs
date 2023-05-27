namespace FaceShuffle.Web.Utilities;

public static class ConfigurationExtensions
{
    public static T Get<T>(this IConfiguration @this, string path) where T : new()
    {
        var instance = new T();
        @this.GetSection(path).Bind(instance);
        return instance;
    }
}
