using Microsoft.AspNetCore.Http;
using WebBackend.Model.Dto;
using WebBackend.Model.Manager;
using WebBackend.Model.Service;
using WebBackend.Model.Storage;
using WebBackend.Service.Extensions;

namespace WebBackend.Service.Manager;

public class AuthManager(
    IAuthService authService,
    IGeneratorService generatorService,
    ISessionStorage sessionStorage) : IAuthManager
{
    private readonly IAuthService _authService = authService;
    private readonly IGeneratorService _generatorService = generatorService;
    private readonly ISessionStorage _sessionStorage = sessionStorage;
    public async Task<string> TryAuthenticate(LoginDto dto)
    {
        var tokens = await _authService.SendAsync(dto.ToAuthenticateDto());
        var sid = _generatorService.GenerateSid();
        await _sessionStorage.AddSession(sid, tokens!.Token);
        return sid;
    }

    public string CreateWebToken()
    {
        return _generatorService.GenerateWebToken();
    }
}