using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Abstractions.Auth;
using FaceShuffle.Application.Abstractions.Repositories;
using FaceShuffle.Application.Actions.Session.BackgroundServices;
using FaceShuffle.Infrastructure;
using FaceShuffle.Infrastructure.Auth;
using FaceShuffle.Infrastructure.Persistence;
using FaceShuffle.Infrastructure.Persistence.Repositories;
using FaceShuffle.Web.Utilities;
using Microsoft.EntityFrameworkCore;

namespace FaceShuffle.Web.DependenciesInstallers;

public static class InfrastructureInstaller
{
    public static void AddInfrastructure(this IServiceCollection @this, IConfiguration configuration)
    {
        @this.AddRepositories();
        @this.AddBackgroundServices();
        @this.AddDbContext(configuration); 
        @this.AddServices();
        @this.AddLazyServices();
        @this.AddOptionalDependencies();
        @this.AddFluentValidation();
    }

    static void AddServices(this IServiceCollection @this)
    {
        @this.AddSingleton<IAuthService, AuthService>();
        @this.AddTransient(typeof(IRequestSender<,>), typeof(MediatRRequestSender<,>));
        @this.AddScoped<IDomainEventsCollector, DomainEventsCollector>();
    }

    static void AddDbContext(this IServiceCollection @this, IConfiguration configuration)
    {
        @this.AddDbContext<RawAppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), x => x.EnableRetryOnFailure()));
        @this.AddTransient<IAppDbContext, AppDbContext>();
    }

    static void AddRepositories(this IServiceCollection @this)
    {
        @this.AddScoped<IUserSessionRepository, UserSessionRepository>();
    }

    static void AddBackgroundServices(this IServiceCollection @this)
    {
        @this.AddTransient(typeof(ApplicationHostedServiceWrapper<>));
        @this.AddApplicationHostedService<InvalidateExpiredSessionsBackgroundService>();
    }

    static void AddApplicationHostedService<T>(this IServiceCollection @this) where T : class, IBackgroundService
    {
        @this.AddTransient<T>();
        @this.AddHostedService(services => services.GetRequiredService<ApplicationHostedServiceWrapper<T>>());
    }
}
