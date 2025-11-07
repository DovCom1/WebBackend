namespace WebBackend.Model.Dto;

public record RegisterDto(string Email, string Password, string Nickname, string Uid, string DateOfBirth, string Gender);