using FaceShuffle.Models.Session.Validators;
using FaceShuffle.Models.Validation;

namespace FaceShuffle.Models.Session;
public class UserFullName
{
    public string Value { get; }

    public UserFullName(string value)
    {
        Value = value;
        this.ValidatePropertyAndThrow(x => x.Value,x=> x.IsUserFullName());
    }
}
