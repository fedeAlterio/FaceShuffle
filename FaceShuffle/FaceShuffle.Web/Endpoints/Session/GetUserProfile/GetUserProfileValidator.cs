using FaceShuffle.Models.Session.Validators;
using FluentValidation;

namespace FaceShuffle.Web.Endpoints.Session.GetUserProfile;
public class GetUserProfileWebRequestValidator : AbstractValidator<GetUserProfileWebRequest>
{
    public GetUserProfileWebRequestValidator()
    {
        RuleFor(x => x.Username)
            .IsUserSessionUsername();
    }
}

