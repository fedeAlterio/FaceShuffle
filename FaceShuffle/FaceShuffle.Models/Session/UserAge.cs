using FaceShuffle.Models.Session.Validators;
using FaceShuffle.Models.Validation;

namespace FaceShuffle.Models.Session;

public record UserAge
{
    public int Value { get; }

    public UserAge(int value)
    {
        Value = value;
        this.ValidatePropertyAndThrow(x => x.Value, x => x.IsUserAge());
    }
}