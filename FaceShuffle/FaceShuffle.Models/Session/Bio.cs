using FaceShuffle.Models.Session.Validators;
using FaceShuffle.Models.Validation;

namespace FaceShuffle.Models.Session;

public record Bio
{
    public Bio(string value)
    {
        Value = value;
        this.ValidatePropertyAndThrow(x => x.Value, x => x.IsBioText());
    }

    public string Value { get; }
}
