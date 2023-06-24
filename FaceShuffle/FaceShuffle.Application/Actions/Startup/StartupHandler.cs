using FaceShuffle.Models.Events;
using MediatR;

namespace FaceShuffle.Application.Actions.Startup;
public class StartupHandler : IRequestHandler<StartupRequest, StartupResponse>
{
    private readonly IPublisher _publisher;

    public StartupHandler(IPublisher publisher)
    {
        _publisher = publisher;
    }
    
    public async Task<StartupResponse> Handle(StartupRequest request, CancellationToken cancellationToken)
    {
        await _publisher.Publish(new ApplicationStartingEvent(), cancellationToken);
        return new();
    }
}
