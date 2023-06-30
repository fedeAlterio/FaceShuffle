using FluentValidation;

namespace FaceShuffle.Models.Session.Validators;
public static class UserAgeValidator
{
    public const int MinimumAge = 18;
    public const int MaximumAge = 130;
    public static IRuleBuilderOptions<T, int> IsUserAge<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .InclusiveBetween(MinimumAge, MaximumAge);
    }
}
