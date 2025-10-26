using WebBackend.Model.Storage;

namespace WebBackend.Service.Storage;

public class WebTokenStorage : IWebTokenStorage
{
    private Dictionary<string, string> _tokens = new Dictionary<string, string>();
    public void SaveWebToken(string token, string sid)
    {
        _tokens[token] = sid;
    }

    public bool WebTokenIsExists(string token)
    {
        if (_tokens.TryGetValue(token, out var sid))
        {
            DeleteWebToken(token);
            return true;
        }

        return false;
    }

    private void DeleteWebToken(string token)
    {
        _tokens.Remove(token);
    }
}