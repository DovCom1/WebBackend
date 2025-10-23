using Microsoft.Extensions.DependencyInjection;
using WebBackend.Model.Manager;
using WebBackend.Model.Service;
using WebBackend.Service.Manager;
using WebBackend.Service.Service;

namespace WebBackend.Service.DependencyInjection;

public static class DependencyExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
            .AddStorages()
            .AddServices()
            .AddManagers();
    }

    private static IServiceCollection AddServices(this  IServiceCollection services)
    {
        return services.AddSingleton<IAuthService, AuthService>()
            .AddSingleton<IGeneratorService, GeneratorService>();
    }

    private static IServiceCollection AddStorages(this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection AddManagers(this IServiceCollection services)
    {
        return services.AddSingleton<IAuthManager, AuthManager>();
    }
}