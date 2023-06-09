namespace FaceShuffle.Models.Exceptions;
public class ExceptionBase : Exception
{
    public ExceptionBase(string message, Exception? innerException = null) : base(message, innerException)
    {
        
    }
}
