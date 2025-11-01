namespace WebBackend.Model.Storage;

public interface IWebTokenStorage
{
    void SaveWebToken(string sid, string token);
    
    bool TryGetSid(string token, out string sid);
}