using System.Security.Claims;

namespace WebBackend.Model.Manager;

public interface ITokenManager
{
    ClaimsPrincipal GetPrincipal(string token);
}