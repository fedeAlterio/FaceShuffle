using FluentValidation;

namespace FaceShuffle.Models.Messages.Validators;
public static class MessageTextContentValidator
{
    public const int MinimumLength = 1;
    public const int MaximumLength = 500;

    public static IRuleBuilderOptions<T, string> IsMessageTextContent<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .MinimumLength(MinimumLength)
            .MaximumLength(MaximumLength);
    }
}
