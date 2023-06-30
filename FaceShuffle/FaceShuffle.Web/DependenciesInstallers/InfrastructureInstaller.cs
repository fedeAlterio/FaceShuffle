using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Abstractions.Auth;
using FaceShuffle.Application.Actions.PendingJobs;
using FaceShuffle.Application.Actions.PendingJobs.BackgroundServices;
using FaceShuffle.Application.Actions.PendingJobs.Configuration;
using FaceShuffle.Application.Actions.Session.BackgroundServices;
using FaceShuffle.Application.Configuration;
using FaceShuffle.Application.Repositories;
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
        @this.PreparedBackgroundServices();
        @this.AddDbContext(configuration); 

        @this.AddUserSession();
        @this.AddPendingJobs(configuration);
        @this.AddMediatRServices();
        @this.AddAuthServices();

        @this.AddMediatRServices();
        @this.AddLazyServices();
        @this.AddOptionalDependencies();
        @this.AddFluentValidation();
    }

    static void AddMediatRServices(this IServiceCollection @this)
    {
        @this.AddTransient(typeof(IRequestSender<,>), typeof(MediatRRequestSender<,>));
        @this.AddScoped<IDomainEventsCollector, DomainEventsCollector>();
    }

    static void AddAuthServices(this IServiceCollection @this)
    {
        @this.AddSingleton<IAuthService, AuthService>();
    }

    static void AddDbContext(this IServiceCollection @this, IConfiguration configuration)
    {
        @this.AddDbContext<RawAppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        @this.AddTransient<IAppDbContext, AppDbContext>();
    }

    static void PreparedBackgroundServices(this IServiceCollection @this)
    {
        @this.AddTransient(typeof(ApplicationHostedServiceWrapper<>));
    }

    static void AddApplicationBackgroundService<T>(this IServiceCollection @this) where T : class, IBackgroundService
    {
        @this.AddSingleton<T>();
        @this.AddHostedService(services => services.GetRequiredService<ApplicationHostedServiceWrapper<T>>());
    }

    static void AddUserSession(this IServiceCollection @this)
    {
        @this.AddScoped<IUserSessionRepository, UserSessionRepository>();
        @this.AddScoped<IUserPicturesRepository, UserPictureFileSystemRepository>();
        @this.AddApplicationBackgroundService<InvalidateExpiredSessionsBackgroundService>();
        @this.AddScoped(service =>
        {
            var hostingEnvironment = service.GetRequiredService<IWebHostEnvironment>();
            return new UserPicturesConfiguration
            {
                Root = Path.Combine(hostingEnvironment.WebRootPath, "SessionPictures")
            };
        });
    }

    static void AddPendingJobs(this IServiceCollection @this, IConfiguration configuration)
    {
        @this.AddScoped<IPendingJobsRepository, PendingJobsRepository>();
        @this.AddScoped<IPendingJobService, PendingJobsService>();
        @this.AddApplicationBackgroundService<ExecutePendingJobsBackgroundService>();
        @this.Configure<PendingJobsConfiguration>(configuration.GetSection("PendingJobs"));
    }
}
