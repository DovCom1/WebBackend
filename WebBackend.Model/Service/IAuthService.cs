using WebBackend.Model.Dto;

namespace WebBackend.Model.Service;

public interface IAuthService
{
    Task<bool> SendAsync(AuthenticateDto dto);
}