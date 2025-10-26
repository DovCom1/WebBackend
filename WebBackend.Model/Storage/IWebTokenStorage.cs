namespace WebBackend.Model.Storage;

public interface IWebTokenStorage
{
    void SaveWebToken(string sid, string token);
    
    bool WebTokenIsExists(string token);
}