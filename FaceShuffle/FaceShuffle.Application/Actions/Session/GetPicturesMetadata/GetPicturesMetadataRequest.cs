using FaceShuffle.Models.Session;
using MediatR;

namespace FaceShuffle.Application.Actions.Session.GetPicturesMetadata;
public class GetPicturesMetadataRequest : IRequest<GetPicturesMetadataResponse>
{
    public required UserSessionGuid UserSessionGuid { get; init; }
}
