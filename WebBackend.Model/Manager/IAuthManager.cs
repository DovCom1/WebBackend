using WebBackend.Model.Dto;

namespace WebBackend.Model.Manager;

public interface IAuthManager
{
    Task<bool> TryAuthenticate(AuthenticateDto dto);
}