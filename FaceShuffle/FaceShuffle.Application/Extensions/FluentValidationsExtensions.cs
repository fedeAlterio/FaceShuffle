using FluentValidation;

namespace FaceShuffle.Application.Extensions;
public static class FluentValidationsExtensions
{
    public static IRuleBuilderOptions<T, string> NotWhiteSpaces<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Matches(@"\A\S+\z");
    }
}
