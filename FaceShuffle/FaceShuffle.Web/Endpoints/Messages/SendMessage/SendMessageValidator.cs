using FaceShuffle.Models.Messages.Validators;
using FaceShuffle.Models.Session.Validators;
using FluentValidation;

namespace FaceShuffle.Web.Endpoints.Messages.SendMessage;
public class SendMessageWebRequestValidator : AbstractValidator<SendMessageWebRequest>
{
    public SendMessageWebRequestValidator()
    {
        RuleFor(x => x.MessageTextContent).IsMessageTextContent();
        RuleFor(x => x.Username).IsUserSessionUsername();
    }
}

