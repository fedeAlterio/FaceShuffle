using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Commands;
using FaceShuffle.Web.DTO;
using MediatR;

namespace FaceShuffle.Web.Endpoints.CreateSession;

public class CreateSessionWebHandler : IRequestHandler<CreateSessionWebRequest, CreateSessionWebResponse>
{
    private readonly IRequestSender<CreateSessionRequest, CreateSessionResponse> _handler;

    public CreateSessionWebHandler(IRequestSender<CreateSessionRequest, CreateSessionResponse> handler)
    {
        _handler = handler;
    }

    public async Task<CreateSessionWebResponse> Handle(CreateSessionWebRequest webRequest, CancellationToken cancellationToken)
    {
        var request = new CreateSessionRequest
        {
            Name = webRequest.Name,
        };

        var response = await _handler.Send(request, cancellationToken);

        var webResponse = new CreateSessionWebResponse
        {
            UserSession = response.UserSession.ToUserSessionDto(),
            JsonWebToken = response.JsonWebToken
        };

        return webResponse;
    }
}
