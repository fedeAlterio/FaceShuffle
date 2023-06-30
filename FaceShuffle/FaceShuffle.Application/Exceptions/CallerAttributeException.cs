namespace FaceShuffle.Application.Exceptions;
public class CallerAttributeException : Exception
{
    public CallerAttributeException(string callerExpression, Exception? innerException) : base(callerExpression, innerException)
    {
        CallerExpression = callerExpression;
    }

    public string CallerExpression { get; }
}
