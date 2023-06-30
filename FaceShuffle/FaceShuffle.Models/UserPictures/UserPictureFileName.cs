using FaceShuffle.Models.UserPictures.Validators;
using FaceShuffle.Models.Validation;

namespace FaceShuffle.Models.UserPictures;
public class UserPictureFileName
{
    public string Value { get; }

    public UserPictureFileName(string value)
    {
        Value = value;
        this.ValidatePropertyAndThrow(x => x.Value, x => x.IsUserPictureFileName());
    }
}
