using FaceShuffle.Models.Exceptions;
using FluentValidation;

namespace FaceShuffle.Models.Extensions;
public static class FluentValidationsExtensions
{
    public static IRuleBuilderOptions<T, string> NotWhiteSpaces<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Matches(@"\A\S+\z").WithMessage(x => $"Ensure {x} does not contains whitespaces");
    }

    public static IRuleBuilderOptions<T, string> LowerCaseOnly<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Matches(@"^[a-z0-9]+").WithMessage("Ensure it is in lower case");
    }

    internal static void ValidateDomainAndThrow<T>(this IValidator<T> @this, T instance)
    {
        try
        {
            @this.ValidateAndThrow(instance);
        }
        catch (ValidationException e)
        {
            throw new DomainValidationException(e);
        }
    }
}
