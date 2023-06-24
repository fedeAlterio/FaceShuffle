using FaceShuffle.Models.Extensions;
using FaceShuffle.Models.Session.Validators;
using FluentValidation;

namespace FaceShuffle.Models.Session;
public record Username
{
    public Username(string value)
    {
        Value = value;
        new UsernameValidator().ValidateDomainAndThrow(this);
    }

    class UsernameValidator : AbstractValidator<Username>
    {
        public UsernameValidator()
        {
            RuleFor(x => x.Value).IsUserSessionUsername();
        }
    }

    public string Value { get; }
}
