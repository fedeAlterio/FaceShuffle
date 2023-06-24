using System.Text.Json;
using FaceShuffle.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FaceShuffle.Application.Extensions;
public static class DbSetExtensions
{
    public static async Task<T> FindAsyncOrThrow<T>(this DbSet<T> @this, object?[]? keyValues, CancellationToken cancellationToken) where T : class
    {
        var ret = await @this.FindAsync(keyValues, cancellationToken);
        return ret ??
               throw new AppException($"Failed to find a {typeof(T)} with key {JsonSerializer.Serialize(keyValues)}");
    }
}
