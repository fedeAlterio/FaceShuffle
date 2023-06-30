using FluentValidation;

namespace FaceShuffle.Models.Generic.Validators;
public static class FileNameValidator
{
    public static IRuleBuilderOptions<T, string> IsValidFileName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull()
            .NotEmpty()
            .Must(x => x.IndexOfAny(Path.GetInvalidPathChars()) < 0)
            .WithMessage("Invalid file name");
    }
}
