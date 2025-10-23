using WebBackend.Model.Storage;

namespace WebBackend.Service.Storage;

public class RedisSessionStorage : ISessionStorage
{
    public Task<bool> AddSession(string sessionId, string token)
    {
        throw new NotImplementedException();
    }
}