using FaceShuffle.Application.Extensions;
using FaceShuffle.Models;
using FluentValidation;

namespace FaceShuffle.Web.Endpoints.Session.CreateSession;

public class CreateSessionWebRequestValidator : AbstractValidator<CreateSessionWebRequest>
{
    public CreateSessionWebRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotWhiteSpaces().WithMessage($"Ensure {nameof(CreateSessionWebRequest.Name)} does not contains whitespaces")
            .MaximumLength(UserSession.NameMaximumLength);  
    }
}
