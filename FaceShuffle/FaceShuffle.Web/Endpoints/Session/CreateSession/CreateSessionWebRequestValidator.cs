using FaceShuffle.Models.Session.Validators;
using FluentValidation;

namespace FaceShuffle.Web.Endpoints.Session.CreateSession;

public class CreateSessionWebRequestValidator : AbstractValidator<CreateSessionWebRequest>
{
    public CreateSessionWebRequestValidator()
    {
        RuleFor(x => x.Username).IsUserSessionUsername();
    }
}
