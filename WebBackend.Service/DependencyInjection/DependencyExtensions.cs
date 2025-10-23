using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using WebBackend.Model.Manager;
using WebBackend.Model.Service;
using WebBackend.Model.Storage;
using WebBackend.Service.Manager;
using WebBackend.Service.Service;
using WebBackend.Service.Storage;

namespace WebBackend.Service.DependencyInjection;

public static class DependencyExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration["RedisConnection:ConnectionString"] 
                               ?? throw new NullReferenceException("No connection string cache");
        var connectionPassword = configuration["RedisConnection:Password"] 
                                 ??  throw new NullReferenceException("No connection password");
        return services
            .AddStorages(connectionString, connectionPassword)
            .AddServices()
            .AddManagers();
    }

    private static IServiceCollection AddServices(this  IServiceCollection services)
    {
        return services.AddSingleton<IAuthService, AuthService>()
            .AddSingleton<IGeneratorService, GeneratorService>();
    }

    private static IServiceCollection AddStorages(
        this IServiceCollection services, 
        string connectionString, 
        string connectionPassword)
    {
        return services
            .AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var options = ConfigurationOptions.Parse(connectionString);
            if (!string.IsNullOrWhiteSpace(connectionString)) options.Password = connectionPassword;
            options.AbortOnConnectFail = false;
            return ConnectionMultiplexer.Connect(options);
        })
            .AddScoped<ISessionStorage, RedisSessionStorage>();
    }

    private static IServiceCollection AddManagers(this IServiceCollection services)
    {
        return services.AddScoped<IAuthManager, AuthManager>();
    }
}