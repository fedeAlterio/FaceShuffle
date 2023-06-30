using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.Session.AddPicture;
using FaceShuffle.Models;
using MediatR;

namespace FaceShuffle.Web.Endpoints.Session.AddUserPicture;

public class AddUserPictureHandler : IRequestHandler<AddUserPictureWebRequest, AddUserPictureWebResponse>
{
    private readonly UserIdentity _userIdentity;
    private readonly IRequestSender<AddPictureRequest, AddPictureResponse> _addPictureSender;

    public AddUserPictureHandler(UserIdentity userIdentity,
        IRequestSender<AddPictureRequest, AddPictureResponse> addPictureSender)
    {
        _userIdentity = userIdentity;
        _addPictureSender = addPictureSender;
    }

    public async Task<AddUserPictureWebResponse> Handle(AddUserPictureWebRequest webRequest, CancellationToken cancellationToken)
    {
        var picture = webRequest.PictureFile;
        var request = new AddPictureRequest
        {
            FileName = new(picture.FileName),
            PictureStream = picture.OpenReadStream(),
            UserPictureSizeBytes = new(picture.Length),
            SessionGuid = _userIdentity.UserSessionGuid
        };

        await _addPictureSender.Send(request, cancellationToken);

        return new();
    }
}
