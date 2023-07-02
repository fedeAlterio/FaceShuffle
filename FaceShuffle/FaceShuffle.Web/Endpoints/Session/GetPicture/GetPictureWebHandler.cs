using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.Session.GetPicture;
using FaceShuffle.Models;
using MediatR;

namespace FaceShuffle.Web.Endpoints.Session.GetPicture;
public class GetPictureWebHandler : IRequestHandler<GetPictureWebRequest, GetPictureWebResponse>
{
    private readonly UserIdentity _userIdentity;
    private readonly IRequestSender<GetPictureRequest, GetPictureResponse> _requestSender;

    public GetPictureWebHandler(
        UserIdentity userIdentity,
        IRequestSender<GetPictureRequest, GetPictureResponse> requestSender)
    {
        _userIdentity = userIdentity;
        _requestSender = requestSender;
    }

    public async Task<GetPictureWebResponse> Handle(GetPictureWebRequest webRequest, CancellationToken cancellationToken)
    {
        var response = await _requestSender.Send(new()
        {
            UserSessionGuid = _userIdentity.UserSessionGuid,
            UserPictureGuid = new(webRequest.UserPictureGuid)
        });

        var result = Results.File(response.PictureStream, response.PictureMetadata.ContentType);
        
        return new()
        {
            Result = result
        };
    }
}
