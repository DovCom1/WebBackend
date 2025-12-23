using Microsoft.AspNetCore.Http;
using WebBackend.Model.Dto;

namespace WebBackend.Model.Manager;

public interface IAuthManager
{
    Task<(string, string)> TryAuthenticate(LoginDto dto);
    Task<bool> TryRegister(RegisterDto dto);
    Task<string?> TryAuthToSid(HttpRequest request);
}