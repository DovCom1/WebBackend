using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using WebBackend.Model.Configs;
using WebBackend.Model.Manager;
using WebBackend.Model.Service;
using WebBackend.Model.Storage;
using WebBackend.Service.Manager;
using WebBackend.Service.Service;
using WebBackend.Service.Storage;
using Microsoft.AspNetCore.Cors;
using WebBackend.Model.Request;

namespace WebBackend.Service.DependencyInjection;

public static class DependencyExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddConfiguration(configuration)
            //.AddStorages(configuration)
            .AddServices()
            .AddManagers()
            .AddHttpClients()
            .AddCorsConfiguration(configuration)
            .AddScoped<ISessionStorage, RedisSessionStorage>();
    }

    private static IServiceCollection AddConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ConductorConfig>(configuration.GetSection("Conductor"));
        services.Configure<AuthConfig>(configuration.GetSection("RequestDomains:AuthService"));
        services.Configure<CorsConfig>(configuration.GetSection("Cors"));

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IGeneratorService, GeneratorService>()
            .AddScoped<IConductorService, ConductorService>()
            .AddSingleton<IAuthService, AuthService>()
            .AddSingleton<RequestFactory>();
    }

    private static IServiceCollection AddStorages(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration["RedisConnection:ConnectionString"]
                               ?? throw new NullReferenceException("No connection string cache");
        var connectionPassword = configuration["RedisConnection:Password"]
                                 ?? throw new NullReferenceException("No connection password");

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

    private static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<IConductorService, ConductorService>((provider, client) =>
        {
            var config = provider.GetRequiredService<IOptions<ConductorConfig>>().Value;
            client.BaseAddress = new Uri(config.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddHttpClient("AuthService", (provider, client) =>
        {
            var config = provider.GetRequiredService<IOptions<ConductorConfig>>().Value;
            client.BaseAddress = new Uri(config.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        return services;
    }

    private static IServiceCollection AddCorsConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var corsConfig = configuration.GetSection("Cors").Get<CorsConfig>() ?? new CorsConfig();
        ;
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(corsConfig.AllowedOrigins)
                    .WithMethods(corsConfig.AllowedMethods)
                    .AllowCredentials();
            });
        });

        return services;
    }
}
