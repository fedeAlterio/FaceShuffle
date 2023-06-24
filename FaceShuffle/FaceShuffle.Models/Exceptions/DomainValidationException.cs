namespace FaceShuffle.Models.Exceptions;
public class DomainValidationException : AppException
{
    public DomainValidationException(Exception innerException) : base("Domain validation error", innerException)
    {
        
    }
}
