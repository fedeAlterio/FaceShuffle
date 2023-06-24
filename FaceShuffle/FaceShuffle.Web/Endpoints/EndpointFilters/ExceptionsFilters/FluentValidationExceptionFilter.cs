using FaceShuffle.Web.DTO;
using FluentValidation;
using FluentValidation.Results;

namespace FaceShuffle.Web.Endpoints.EndpointFilters.ExceptionsFilters;

public class FluentValidationExceptionFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (ValidationException validationException)
        {
            return ValidationErrorResult(validationException.Errors);
        }
    }

    FieldValidationErrorsInfo ValidationErrorResult(IEnumerable<ValidationFailure> validationExceptionErrors)
    {
        IEnumerable<FieldValidationError> GetErrors()
        {
            foreach (var x in validationExceptionErrors)
            {
                yield return new FieldValidationError { FieldName = x.PropertyName, Message = x.ErrorMessage };
            }
        }

        var fieldValidationError = new FieldValidationErrorsInfo { ValidationErrors = GetErrors().ToList() };

        return fieldValidationError;
    }
}
