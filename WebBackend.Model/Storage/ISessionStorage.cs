namespace WebBackend.Model.Storage;

public interface ISessionStorage
{
    Task<bool> AddSession(string sessionId, string token);
}