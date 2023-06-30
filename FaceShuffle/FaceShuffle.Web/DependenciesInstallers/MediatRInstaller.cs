using FaceShuffle.Application.PipelineBehaviors;
using MediatR;
using MediatR.NotificationPublishers;

namespace FaceShuffle.Web.DependenciesInstallers;

public static class MediatRInstaller
{
    public static void AddMediatR(this IServiceCollection @this)
    {
        @this.AddMediatR(x =>
        {
            x.NotificationPublisher = new ForeachAwaitPublisher();
            x.RegisterServicesFromAssemblies(typeof(IAssemblyMarker).Assembly, typeof(Application.IAssemblyMarker).Assembly);
        });

        @this.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkPipelineBehavior<,>));
        @this.AddScoped(typeof(IPipelineBehavior<,>), typeof(PublishDomainEventsPipelineBehavior<,>));
        @this.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
    }
}
