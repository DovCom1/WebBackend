using WebBackend.Model.Dto;

namespace WebBackend.Service.Extensions;

public static class DtoExtensions
{
    public static AuthenticateDto ToAuthenticateDto(this LoginDto dto) 
        => new AuthenticateDto(dto.Email, dto.Password);
    
    public static AuthenticateDto ToAuthenticateDto(this RegisterDto dto)
        => new AuthenticateDto(dto.Email, dto.Password);
    
    public static UserIdDto ToUserIdDto(this UserInfoDto dto)
        => new UserIdDto(dto.Id, dto.Email);
}