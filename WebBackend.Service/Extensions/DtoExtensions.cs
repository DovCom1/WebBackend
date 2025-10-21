using WebBackend.Model.Dto;

namespace WebBackend.Service.Extensions;

public static class DtoExtensions
{
    public static AuthenticateDto ToAuthenticateDto(this LoginDto dto) 
        => new AuthenticateDto(dto.Email, dto.Password);
}