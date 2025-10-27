using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using WebBackend.Model.Manager;
using WebBackend.Model.Service;
using WebBackend.Model.Storage;
using WebBackend.Service.Manager;
using WebBackend.Service.Service;
using WebBackend.Service.Storage;
using WebBackend.Model.Request;


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
                                 ?? throw new NullReferenceException("No connection password");
        return services
            .AddOptions(configuration)
            .AddStorages(connectionString, connectionPassword)
            .AddServices()
            .AddManagers()
            .AddCorsConfiguration(configuration)
            .AddHttpClientFactory();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
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
            .AddScoped<ISessionStorage, RedisSessionStorage>()
            .AddSingleton<IWebTokenStorage, WebTokenStorage>();
    }

    private static IServiceCollection AddManagers(this IServiceCollection services)
    {
        return services.AddScoped<IAuthManager, AuthManager>();
    }

    private static IServiceCollection AddHttpClientFactory(this IServiceCollection services)
    {
        services
            .AddSingleton<RequestFactory, RequestFactory>()
            .AddHttpClient();
        return services;
    }

    private static IServiceCollection AddCorsConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("frontend", p => p
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromHours(1))
            );
        });

        return services;
    }

    private static IServiceCollection AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .Configure<RequestDomains>(configuration.GetSection("RequestDomains"));
    }
}