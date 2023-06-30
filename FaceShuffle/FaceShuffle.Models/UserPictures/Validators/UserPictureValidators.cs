using FaceShuffle.Models.Generic.Validators;
using FluentValidation;

namespace FaceShuffle.Models.UserPictures.Validators;
public static class UserPictureValidators
{
    public const int PictureFileNameMaximumLength = 35;
    public const int PictureFileNameMinimumLength = 3;
    public const int MaximumPictureSize = 1024 * 1024 * 5;

    public static IRuleBuilderOptions<T, string> IsUserPictureFileName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .IsValidFileName()
            .HasImageExtension()
            .MaximumLength(PictureFileNameMaximumLength)
            .MinimumLength(PictureFileNameMinimumLength);
    }

    public static IRuleBuilderOptions<T, long> IsUserPictureLength<T>(this IRuleBuilder<T, long> ruleBuilder)
    {
        return ruleBuilder
            .InclusiveBetween(1, MaximumPictureSize);
    }
}
