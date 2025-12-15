namespace WebBackend.Model.Storage;

public interface ISessionStorage
{
    Task<bool> AddSession(string sessionId, string token);
    Task<string?> GetAccessToken(string sessionId);
    Task AddUserId(string userId, string sessionId);
    Task<List<string>> GetUserIds(string userId);
}