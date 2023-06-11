using System.Diagnostics.CodeAnalysis;
using Optional;
using Optional.Unsafe;

namespace FaceShuffle.Application.Extensions;
public static class OptionsExtensions
{
    public static bool TryGetValue<T>(this Option<T> @this, [NotNullWhen(true)] out T value)
    {
        value = @this.ValueOrDefault();
        return @this.HasValue;
    }
}
