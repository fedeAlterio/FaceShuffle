using FaceShuffle.Web.DTO;

namespace FaceShuffle.Web.Endpoints.EndpointFilters;

public record FieldValidationErrorsInfo
{
    public IReadOnlyList<FieldValidationError> ValidationErrors { get; init; } = new List<FieldValidationError>();
}
