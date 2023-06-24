using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.Secret.GetSecret;
using MediatR;

namespace FaceShuffle.Web.Endpoints.Secret;

public class SecretWebHandler : IRequestHandler<SecretWebRequest, SecretWebResponse>
{
    private readonly IRequestSender<SecretRequest, SecretResponse> _sender;

    public SecretWebHandler(IRequestSender<SecretRequest, SecretResponse> sender)
    {
        _sender = sender;
    }

    public async Task<SecretWebResponse> Handle(SecretWebRequest webRequest, CancellationToken cancellationToken)
    {
        var request = new SecretRequest();
        var response = await _sender.Send(request, cancellationToken);
        return new()
        {
            Secret = response.Secret,
        };
    }
}
