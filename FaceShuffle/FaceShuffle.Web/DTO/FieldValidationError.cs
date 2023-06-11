namespace FaceShuffle.Web.DTO;

public class FieldValidationError
{
    public required string FieldName { get; init; }
    public required string Message { get; init; }
}
