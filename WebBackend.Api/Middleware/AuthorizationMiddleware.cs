using WebBackend.Model.Constants;
using WebBackend.Model.Storage;

namespace WebBackend.Api.Middleware;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizationMiddleware> _logger;

    private readonly string[] _publicEndpoints =
    {
        "/auth/login",
        "/auth/register",
        "/swagger"
    };

    public AuthorizationMiddleware(
        RequestDelegate next,
        ILogger<AuthorizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ISessionStorage sessionStorage)
    {
        if (ShouldSkipAuthorization(context))
        {
            await _next(context);
            return;
        }

        var isAuthenticated = await AuthenticateUserAsync(context, sessionStorage);

        if (!isAuthenticated)
        {
            _logger.LogWarning(
                "Authentication failed for {Method} {Path}.",
                context.Request.Method,
                context.Request.Path);

            await WriteUnauthorizedResponseAsync(context);
            return;
        }

        await _next(context);
    }

    private bool ShouldSkipAuthorization(HttpContext context)
    {
        var path = context.Request.Path;
        return _publicEndpoints.Any(publicPath => path.StartsWithSegments(publicPath)) ||
               context.Request.Method == HttpMethods.Options;
    }

    private async Task<bool> AuthenticateUserAsync(
        HttpContext context,
        ISessionStorage sessionStorage)
    {
        var sessionId = ExtractSessionId(context);
        if (!string.IsNullOrEmpty(sessionId))
        {
            var token = await sessionStorage.GetAccessToken(sessionId);
            if (!string.IsNullOrEmpty(token))
            {
                _logger.LogDebug("Found token for session {SessionId}", sessionId);
                return true;
            }
            else
            {
                _logger.LogDebug("No token found for session {SessionId}", sessionId);
            }
        }
        return false;
    }
    
    private static string? ExtractSessionId(HttpContext context)
    {
        return context.Request.Cookies.TryGetValue(Constants.SidName, out var sessionId)
            ? sessionId
            : null;
    }
    
    private static Task WriteUnauthorizedResponseAsync(HttpContext context)
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";

        var response = new { error = "Authentication required" };
        var json = System.Text.Json.JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(json);
    }
}