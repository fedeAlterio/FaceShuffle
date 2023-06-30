using FluentValidation;

namespace FaceShuffle.Models.Generic.Validators;
public static class HasImageExtensionValidator
{
    private static readonly IReadOnlyCollection<string> PossibleImagesExtensions = new HashSet<string>
    {
        ".jpg",
        ".jpeg",
        ".bmp",
        ".png"
    };

    private static readonly Lazy<string> PossibleImagesErrorMessage = new(() =>
        $"Only the following extensions are allowed: [{string.Join(", ", PossibleImagesExtensions)}]");

    public static IRuleBuilderOptions<T, string> HasImageExtension<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(fileName =>
            {
                var extension = Path.GetExtension(fileName).ToLower();
                return PossibleImagesExtensions.Contains(extension);
            })
            .WithMessage(PossibleImagesErrorMessage.Value);
    }
}
