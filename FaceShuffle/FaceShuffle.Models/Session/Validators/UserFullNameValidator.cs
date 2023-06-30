using FaceShuffle.Models.Validation;
using FluentValidation;

namespace FaceShuffle.Models.Session.Validators;
public static class UserFullNameValidator
{
    public const int MinimumLength = 2;
    public const int MaximumLength = 20;
    public static IRuleBuilderOptions<T, string> IsUserFullName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .NotWhiteSpaces()
            .MinimumLength(MinimumLength)
            .MaximumLength(MaximumLength);
    }
}
