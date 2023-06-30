using FaceShuffle.Models.Validation;
using FluentValidation;

namespace FaceShuffle.Models.Session.Validators;
public static class BioValidator
{
    public const int MinimumLength = 0;
    public const int MaximumLength = 500;
    public static IRuleBuilderOptions<T, string> IsBioText<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotNull()
            .MinimumLength(MinimumLength)
            .MaximumLength(MaximumLength)
            .IsTrimmed();
    }
}
