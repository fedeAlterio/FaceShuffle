using FaceShuffle.Models.Session.Validators;
using FluentValidation;

namespace FaceShuffle.Web.Endpoints.Session.CreateSession;

public class CreateSessionWebRequestValidator : AbstractValidator<CreateSessionWebRequest>
{
    public CreateSessionWebRequestValidator()
    {
        RuleFor(x => x.Username).IsUserSessionUsername();
        RuleFor(x => x.Bio).IsBioText();
        RuleFor(x => x.UserAge).IsUserAge();
        RuleFor(x => x.UserFullName).IsUserFullName();
    }
}
