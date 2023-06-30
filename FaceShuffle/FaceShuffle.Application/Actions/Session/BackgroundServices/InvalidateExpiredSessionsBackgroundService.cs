using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Abstractions.Auth;
using FaceShuffle.Application.Actions.Session.InvalidateAllExpiredSessions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FaceShuffle.Application.Actions.Session.BackgroundServices;
public class InvalidateExpiredSessionsBackgroundService : IBackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<UserSessionConfiguration> _userSessionConfiguration;
    private readonly ILogger<InvalidateExpiredSessionsBackgroundService> _logger;

    public InvalidateExpiredSessionsBackgroundService(
        IServiceProvider serviceProvider,
        IOptions<UserSessionConfiguration> userSessionConfiguration,
        ILogger<InvalidateExpiredSessionsBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _userSessionConfiguration = userSessionConfiguration;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var period = TimeSpan.FromMinutes(_userSessionConfiguration.Value.MinutesBeforeExpiration);
        using var timer = new PeriodicTimer(period);

        do
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var expiredSessionsInvalidator = scope.ServiceProvider
                    .GetRequiredService<IRequestSender<InvalidateAllExpiredSessionsRequest,
                        InvalidateAllExpiredSessionsResponse>>();

                await expiredSessionsInvalidator.Send(new(), cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to remove expired requests");
            }
        } while (await timer.WaitForNextTickAsync(cancellationToken));
    }
}
