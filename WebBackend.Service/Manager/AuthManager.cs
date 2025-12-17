using System.Text.Json;
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
        IConductorManager conductorManager,
        ISessionStorage sessionStorage) : IAuthManager
    {
    public async Task<(string, string)> TryAuthenticate(LoginDto dto)
    {
        var tokens = await authService.LoginAsync(dto.ToAuthenticateDto());
        var sid = generatorManager.GenerateSid();
        if (tokens.Token == null)
        {
            logger.LogError("Not Access Token");
            throw new Exception("Not Access Token");
        }
        var status = await sessionStorage.AddSession(sid, tokens.Token);
        await sessionStorage.AddUserId(tokens.UserId.ToString(), sid);
        if (!status)
        {
            logger.LogError("Failed to write access token");
            throw new Exception("Failed to write access token");
        }
        return (sid, tokens.UserId);
    }

    public async Task<bool> TryRegister(RegisterDto dto)
    {
        var status = await authService.RegisterAsync(dto.ToAuthenticateDto());
        if (!status)
        {
            return false;
        }
        var response = await conductorManager.SendProxyRequestAsync(HttpMethod.Post, "users", "register", dto);
        if (!response.IsSuccess)
        {
            return false;
        }
        var json = JsonSerializer.Deserialize<UserInfoDto>(response.Content,  
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (json == null)
        {
            return false;
        }
        logger.LogInformation("!!!!!!!!!!!!" + response.Content);
        status = await authService.PutIdAsync(json.ToUserIdDto());
        if (!status)
        {
            return false;
        }
        return true;
    }
}