using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using WebBackend.Api.Options;
using WebBackend.Model.Manager;
using WebBackend.Model.Storage;

namespace WebBackend.Api.Handlers;

public class TokenAuthHandler : AuthenticationHandler<TokenAuthOptions>
{
    private readonly IWebTokenStorage _webTokenStorage;
    private readonly ISessionStorage _sessionStorage;
    private readonly ITokenManager _tokenManager;

    public TokenAuthHandler(
        IOptionsMonitor<TokenAuthOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IWebTokenStorage storage,
        ISessionStorage sessionStorage,
        ITokenManager tokenManager
    ) : base(options, logger, encoder, clock)
    {
        _webTokenStorage = storage;
        _sessionStorage = sessionStorage;
        _tokenManager = tokenManager;
    }
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Request.Query.TryGetValue("web_token", out var webToken)
            && _webTokenStorage.TryGetSid(webToken, out var sid))
        {
            var accessToken = await _sessionStorage.GetAccessToken(sid);
            if (accessToken == null)
            {
                return AuthenticateResult.NoResult();
            }
            var principal = _tokenManager.GetPrincipal(accessToken);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        return AuthenticateResult.NoResult();
    }
}