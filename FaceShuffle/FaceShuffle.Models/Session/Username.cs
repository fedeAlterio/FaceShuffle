using FaceShuffle.Models.Session.Validators;
using FaceShuffle.Models.Validation;

namespace FaceShuffle.Models.Session;
public record Username
{
    public Username(string value)
    {
        Value = value;
        this.ValidatePropertyAndThrow(x => x.Value, x => x.IsUserSessionUsername());
    }

    public string Value { get; }
}
