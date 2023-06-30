using FaceShuffle.Models.UserPictures.Validators;
using FaceShuffle.Models.Validation;

namespace FaceShuffle.Models.UserPictures;
public class UserPictureLength
{
    public long Value { get; }

    public UserPictureLength(long value)
    {
        Value = value;
        this.ValidatePropertyAndThrow(x => x.Value, x => x.IsUserPictureLength());
    }
}
