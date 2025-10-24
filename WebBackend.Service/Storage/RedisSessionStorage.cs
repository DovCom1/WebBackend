using StackExchange.Redis;
using WebBackend.Model.Constants;
using WebBackend.Model.Storage;

namespace WebBackend.Service.Storage;

public class RedisSessionStorage(IConnectionMultiplexer multiplexer) : ISessionStorage
{
    private readonly IDatabase _database = multiplexer.GetDatabase();
    public async Task<bool> AddSession(string sessionId, string token)
    {
        return await _database.StringSetAsync(sessionId, token, RedisConstants.Ttl);
    }

    public async Task<string?> GetToken(string sessionId)
    {
        return await _database.StringGetAsync(sessionId);
    }

    public async Task<bool> RemoveSession(string sessionId)
    {
        return await _database.KeyDeleteAsync(sessionId);
    }
}