using FaceShuffle.Models.Session.Validators;
using FluentValidation;

namespace FaceShuffle.Web.Endpoints.Session.UpdateUserProfile;
public class UpdateUserProfileWebRequestValidator : AbstractValidator<UpdateUserProfileWebRequest>
{
    public UpdateUserProfileWebRequestValidator()
    {
        RuleFor(x => x.Bio).IsBioText();
        RuleFor(x => x.UserAge).IsUserAge();
        RuleFor(x => x.UserFullName).IsUserFullName();
    }
}

