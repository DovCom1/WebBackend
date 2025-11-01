using WebBackend.Model.Dto;
using WebBackend.Model.Manager;
using WebBackend.Model.Service;
using WebBackend.Model.Storage;
using WebBackend.Service.Extensions;

namespace WebBackend.Service.Manager;

public class AuthManager(
    IAuthService authService,
    IGeneratorService generatorService,
    ISessionStorage sessionStorage,
    IWebTokenStorage webTokenStorage) : IAuthManager
{
    private readonly IAuthService _authService = authService;
    private readonly IGeneratorService _generatorService = generatorService;
    private readonly ISessionStorage _sessionStorage = sessionStorage;
    private readonly IWebTokenStorage _webTokenStorage = webTokenStorage;
    public async Task<(string, string)> TryAuthenticate(LoginDto dto)
    {
        var tokens = await _authService.LoginAsync(dto.ToAuthenticateDto());
        var sid = _generatorService.GenerateSid();
        await _sessionStorage.AddSession(sid, tokens!.Token);
        var webToken = CreateWebToken();
        _webTokenStorage.SaveWebToken(sid, webToken);
        return (sid, webToken);
    }

    public async Task<bool> TryRegister(RegisterDto dto)
    {
        return await _authService.RegisterAsync(dto.ToAuthenticateDto());
    }

    public string CreateWebToken()
    {
        return _generatorService.GenerateWebToken();
    }
}