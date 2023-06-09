using FaceShuffle.Models;
using MediatR;

namespace FaceShuffle.Application.Queries;
public class SecretRequestHandler :IRequestHandler<SecretRequest, SecretResponse>
{
    private readonly UserIdentity _userIdentity;

    public SecretRequestHandler(UserIdentity userIdentity)
    {
        _userIdentity = userIdentity;
    }

    public async Task<SecretResponse> Handle(SecretRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return new()
        {
            Secret = $"Secret For {_userIdentity.Name}"
        };
    }
}
