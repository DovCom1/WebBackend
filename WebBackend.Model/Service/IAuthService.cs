using WebBackend.Model.Dto;

namespace WebBackend.Model.Service;

public interface IAuthService
{
    Task<AuthTokenDto?> LoginAsync(AuthenticateDto dto);
    
    Task<bool> RegisterAsync(AuthenticateDto dto);
}