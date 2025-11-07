using Microsoft.Extensions.Logging;
using WebBackend.Model.Dto;
using WebBackend.Model.Manager;
using WebBackend.Model.Service;
using WebBackend.Model.Storage;
using WebBackend.Service.Extensions;

namespace WebBackend.Service.Manager;

    public class AuthManager(
        ILogger<AuthManager> logger,
        IAuthService authService,
        IGeneratorManager generatorManager,
        ISessionStorage sessionStorage) : IAuthManager
    {
    public async Task<string> TryAuthenticate(LoginDto dto)
    {
        var tokens = await authService.LoginAsync(dto.ToAuthenticateDto());
        var sid = generatorManager.GenerateSid();
        if (tokens.Token == null)
        {
            logger.LogError("Not Access Token");
            throw new Exception("Not Access Token");
        }
        var status = await sessionStorage.AddSession(sid, tokens.Token);
        if (!status)
        {
            logger.LogError("Failed to write access token");
            throw new Exception("Failed to write access token");
        }
        return sid;
    }

    public async Task<bool> TryRegister(RegisterDto dto)
    {
        return await authService.RegisterAsync(dto.ToAuthenticateDto());
    }
}