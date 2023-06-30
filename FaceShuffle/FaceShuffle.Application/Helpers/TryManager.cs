using System.Runtime.CompilerServices;
using FaceShuffle.Application.Exceptions;
using FaceShuffle.Models.Generic;

namespace FaceShuffle.Application.Helpers;
public static class TryEx
{
    public static async Task<Optional<Exception>> Try(Func<Task> action, [CallerArgumentExpression(nameof(action))] string expression = "")
    {
        try
        {
            await action().ConfigureAwait(false);
            return default;
        }
        catch (Exception e)
        {
            Exception exception = new CallerAttributeException(expression, e);
            return exception;
        }
    }
}
