using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using WebBackend.Api.Options;
using WebBackend.Model.Manager;
using WebBackend.Model.Storage;

namespace WebBackend.Api.Handlers;

public class TokenAuthHandler : AuthenticationHandler<TokenAuthOptions>
{
    private readonly ISessionStorage _sessionStorage;
    private readonly ITokenManager _tokenManager;

    public TokenAuthHandler(
        IOptionsMonitor<TokenAuthOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        ISessionStorage sessionStorage,
        ITokenManager tokenManager
    ) : base(options, logger, encoder, clock)
    {
        _sessionStorage = sessionStorage;
        _tokenManager = tokenManager;
    }
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Request.Cookies.TryGetValue("dovcom-sid", out var sid);
        if (sid == null)
        {
            Logger.LogWarning("No sid in cookies");
            return AuthenticateResult.NoResult();
        }
            
        var accessToken = await _sessionStorage.GetAccessToken(sid);
        if (accessToken == null)
        {
            Logger.LogWarning("Dont have access token in storage.");
            return AuthenticateResult.NoResult();
        }
        var principal = _tokenManager.GetPrincipal(accessToken);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }
}