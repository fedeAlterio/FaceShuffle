using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Actions.Startup;

namespace FaceShuffle.Web;

public class Startup
{
    private readonly IServiceProvider _serviceProvider;

    public Startup(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartApplication(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var startupSender = scope.ServiceProvider.GetRequiredService<IRequestSender<StartupRequest, StartupResponse>>();
        await startupSender.Send(new(), cancellationToken);
    }
}
