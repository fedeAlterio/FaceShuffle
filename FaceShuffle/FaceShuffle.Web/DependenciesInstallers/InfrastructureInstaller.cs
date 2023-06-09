using FaceShuffle.Application.Abstractions;
using FaceShuffle.Application.Abstractions.Auth;
using FaceShuffle.Application.Abstractions.Repositories;
using FaceShuffle.Infrastructure;
using FaceShuffle.Infrastructure.Auth;
using FaceShuffle.Infrastructure.Persistence;
using FaceShuffle.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FaceShuffle.Web.DependenciesInstallers;

public static class InfrastructureInstaller
{
    public static void AddInfrastructure(this IServiceCollection @this, IConfiguration configuration)
    {
        @this.AddRepositories();
        @this.AddDbContext(configuration);
        @this.AddServices();
    }

    static void AddServices(this IServiceCollection @this)
    {
        @this.AddSingleton<IAuthService, AuthService>();
        @this.AddTransient(typeof(IRequestSender<,>), typeof(MediatRRequestSender<,>));
        @this.AddScoped<IDomainEventsCollector, DomainEventsCollector>();
    }

    static void AddDbContext(this IServiceCollection @this, IConfiguration configuration)
    {
        @this.AddDbContext<RawAppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        @this.AddTransient<IAppDbContext, AppDbContext>();
    }

    static void AddRepositories(this IServiceCollection @this)
    {
        @this.AddScoped<IUserSessionRepository, UserSessionRepository>();
    }
}
