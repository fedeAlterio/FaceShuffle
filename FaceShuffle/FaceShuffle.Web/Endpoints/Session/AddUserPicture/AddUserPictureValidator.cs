using FaceShuffle.Models.UserPictures.Validators;
using FluentValidation;

namespace FaceShuffle.Web.Endpoints.Session.AddUserPicture;

public class AddUserPictureValidator : AbstractValidator<AddUserPictureWebRequest>
{
    public AddUserPictureValidator()
    {
        RuleFor(x => x.PictureFile.FileName)
            .IsUserPictureFileName();

        RuleFor(x => x.PictureFile.Length)
            .IsUserPictureLength();
    }
}
