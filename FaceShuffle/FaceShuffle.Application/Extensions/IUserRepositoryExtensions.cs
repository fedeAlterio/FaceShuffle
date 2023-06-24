using FaceShuffle.Application.Abstractions.Repositories;
using FaceShuffle.Application.Exceptions;
using FaceShuffle.Models.Session;

namespace FaceShuffle.Application.Extensions;
// ReSharper disable once InconsistentNaming
public static class IUserRepositoryExtensions
{
    public static async Task ThrowIfUsernameExists(this IUserSessionRepository @this, Username username, CancellationToken cancellationToken)
    {
        if (await @this.ExistsUsername(username, cancellationToken))
            throw new UserReadableAppException
            {
                UserText = $"Username {username.Value} already in use"
            };
    }
}
