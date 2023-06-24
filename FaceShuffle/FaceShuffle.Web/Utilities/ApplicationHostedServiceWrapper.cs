using FaceShuffle.Application.Abstractions;

namespace FaceShuffle.Web.Utilities;

public class ApplicationHostedServiceWrapper<T> : BackgroundService where T : IBackgroundService
{
    private readonly T _backgroundService;

    public ApplicationHostedServiceWrapper(T backgroundService)
    {
        _backgroundService = backgroundService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return _backgroundService.ExecuteAsync(stoppingToken);
    }
}
