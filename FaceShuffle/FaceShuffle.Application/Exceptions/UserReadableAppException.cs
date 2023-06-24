using FaceShuffle.Models.Exceptions;

namespace FaceShuffle.Application.Exceptions;
public class UserReadableAppException : AppException
{
    public UserReadableAppException() : this("an User readable exception occurred")
    {
        
    }

    public UserReadableAppException(string message, Exception? innerException = null) : base(message, innerException)
    {
    }

    public required string UserText { get; init; }
}
