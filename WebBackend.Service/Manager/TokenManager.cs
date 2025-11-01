using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebBackend.Model.Configuration;
using WebBackend.Model.Manager;

namespace WebBackend.Service.Manager;

public class TokenManager(IOptions<SecretKeys> options) : ITokenManager
{
    private readonly JwtSecurityTokenHandler _handler = new JwtSecurityTokenHandler();
    private readonly string _signingKey = options.Value.TokenSecretKey;
    public ClaimsPrincipal GetPrincipal(string token)
    {
        var options = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = false,
            ValidateLifetime = false,
            ValidateActor = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signingKey))
        };
        return _handler.ValidateToken(token, options, out _);
    }
}