using FaceShuffle.Models.Validation;
using FluentValidation;

namespace FaceShuffle.Models.Session.Validators;
public static class UsernameValidator
{
    public const int MinimumLength = 5;
    public const int MaximumLength = 20;
    public static IRuleBuilderOptions<T, string> IsUserSessionUsername<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .NotWhiteSpaces()
            .MinimumLength(MinimumLength)
            .MaximumLength(MaximumLength)
            .LowerCaseOnly();
    }
}
