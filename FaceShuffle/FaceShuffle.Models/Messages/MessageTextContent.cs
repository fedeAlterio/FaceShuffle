using FaceShuffle.Models.Messages.Validators;
using FaceShuffle.Models.Validation;

namespace FaceShuffle.Models.Messages;
public class MessageTextContent
{
    public string Value { get; }

    public MessageTextContent(string value)
    {
        Value = value;
        this.ValidatePropertyAndThrow(x => x.Value, x => x.IsMessageTextContent());
    }
}
