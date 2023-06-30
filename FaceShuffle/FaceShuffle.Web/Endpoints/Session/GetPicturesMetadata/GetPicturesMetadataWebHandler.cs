using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.Session.GetPicturesMetadata;
using FaceShuffle.Models;
using FaceShuffle.Web.DTO;
using MediatR;

namespace FaceShuffle.Web.Endpoints.Session.GetPicturesMetadata;
public class GetPicturesMetadataWebHandler : IRequestHandler<GetPicturesMetadataWebRequest, GetPicturesMetadataWebResponse>
{
    private readonly UserIdentity _userIdentity;
    private readonly IRequestSender<GetPicturesMetadataRequest, GetPicturesMetadataResponse> _requestSender;

    public GetPicturesMetadataWebHandler(UserIdentity userIdentity, IRequestSender<GetPicturesMetadataRequest, GetPicturesMetadataResponse> requestSender)
    {
        _userIdentity = userIdentity;
        _requestSender = requestSender;
    }

    public async Task<GetPicturesMetadataWebResponse> Handle(GetPicturesMetadataWebRequest webRequest, CancellationToken cancellationToken)
    {
        var request = new GetPicturesMetadataRequest
        {
            UserSessionGuid = _userIdentity.UserSessionGuid
        };

        var response = await _requestSender.Send(request, cancellationToken);

        var picturesMetadataDto = response.PicturesMetadata
            .Select(UserPictureMetadataDto.FromUserPictureMetadata)
            .ToList();

        return new()
        {
            PicturesMetadata = picturesMetadataDto
        };
    }
}
