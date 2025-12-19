namespace WebBackend.Model.Dto;

public record UserInfoDto(
    string Id, 
    string Uid, 
    string Nickname, 
    string Email, 
    string AvatarUrl, 
    string Gender, 
    string Status, 
    string DateOfBirth);