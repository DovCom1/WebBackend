using StackExchange.Redis;
using WebBackend.Api.Handlers;
using WebBackend.Api.Options;
using WebBackend.Model.Configuration;
using WebBackend.Model.Manager;
using WebBackend.Model.Service;
using WebBackend.Model.Storage;
using WebBackend.Service.Manager;
using WebBackend.Service.Service;
using WebBackend.Service.Storage;
using WebBackend.Model.Request;


namespace WebBackend.Api.Extensions;

public static class DependencyExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment)
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
            .AddCorsConfiguration()
            .AddHttpClientFactory()
            .AddSwagger(environment)
            .AddAuth()
            .AddWebSockets();
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
                var mux = ConnectionMultiplexer.ConnectAsync(options).GetAwaiter().GetResult();
                Console.WriteLine("ConnectionMultiplexer connect is: " + mux.IsConnected);
                return mux;
            })
            .AddScoped<ISessionStorage, RedisSessionStorage>();
    }

    private static IServiceCollection AddManagers(this IServiceCollection services)
    {
        return services
            .AddScoped<IAuthManager, AuthManager>()
            .AddScoped<ITokenManager, TokenManager>();
    }

    private static IServiceCollection AddHttpClientFactory(this IServiceCollection services)
    {
        services
            .AddSingleton<RequestFactory, RequestFactory>()
            .AddHttpClient();
        return services;
    }

    private static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
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
            .Configure<RequestDomains>(configuration.GetSection("RequestDomains"))
            .Configure<SecretKeys>(configuration.GetSection("SecretKeys"))
            .Configure<RedisConnection>(configuration.GetSection("RedisConnection"));
    }

    private static IServiceCollection AddSwagger(
        this IServiceCollection services,
        IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        return services;
    }

    private static IServiceCollection AddAuth(
        this IServiceCollection services)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "TokenScheme";
                options.DefaultChallengeScheme = "TokenScheme";
            })
            .AddScheme<TokenAuthOptions, TokenAuthHandler>("TokenScheme", options => { });
        services.AddAuthorization();
        return services;
    }

    private static IServiceCollection AddWebSockets(
        this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }
}