using WebBackend.Model.Dto;
using WebBackend.Model.Manager;
using WebBackend.Model.Service;

namespace WebBackend.Service.Manager;

public class AuthManager(IAuthService authService) : IAuthManager
{
    private readonly IAuthService _authService = authService;
    public async Task<bool> TryAuthenticate(AuthenticateDto dto)
    {
        var status = await authService.SendAsync(dto);
        if (!status)
        {
            
        }
    }
}