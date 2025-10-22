using WebBackend.Model.Dto;

namespace WebBackend.Model.Manager;

public interface IAuthManager
{
    Task<string> TryAuthenticate(LoginDto dto);
    string CreateWebToken();
}