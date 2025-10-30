using WebBackend.Api.Middleware;

namespace WebBackend.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthorizationMiddleware>();
    }
}