using WebBackend.Model.Dto;

namespace WebBackend.Model.Service;

public interface IAuthService
{
    Task<AuthTokenDto?> SendAsync(AuthenticateDto dto);
}