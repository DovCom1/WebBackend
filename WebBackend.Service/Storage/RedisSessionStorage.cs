using StackExchange.Redis;
using WebBackend.Model.Constants;
using WebBackend.Model.Storage;

namespace WebBackend.Service.Storage;

public class RedisSessionStorage(IConnectionMultiplexer multiplexer) : ISessionStorage
{
    private readonly IDatabase _database = multiplexer.GetDatabase();
    public async Task<bool> AddSession(string sessionId, string token)
    {
        return await _database.StringSetAsync(SessionSetKey(sessionId), token, RedisConstants.Ttl);
    }

    public async Task<bool> AddSessionToUserId(string sessionId, string userId)
    {
        return await _database.StringSetAsync(SessionToUserIdSetKey(sessionId), userId, RedisConstants.Ttl);
    }

    public async Task<string?> GetUserIdBySession(string session)
    {
        return await _database.StringGetAsync(SessionToUserIdSetKey(session));
    }

    public async Task<bool> AddConnection(string connectionId)
    {
        return await _database.StringSetAsync(ConnectionIdKey(connectionId), connectionId);
    }

    public async Task<string?> GetConnectionId(string connectionId)
    {
        return await _database.StringGetAsync(ConnectionIdKey(connectionId));
    }

    public async Task<bool> RemoveConnection(string connectionId)
    {
        return await _database.KeyDeleteAsync(ConnectionIdKey(connectionId));
    }

    public async Task AddUserId(string userId, string sessionId)
    {
        await _database.ListRightPushAsync(UserSetkey(userId), sessionId);
    }
    
    public async Task<List<string>> GetUserIds(string userId)
    {
        return (await _database.ListRangeAsync(UserSetkey(userId), 0, -1))
            .Select(x => x.ToString())
            .ToList();
    }

    public async Task<string?> GetAccessToken(string sessionId)
    {
        return await _database.StringGetAsync(SessionSetKey(sessionId));
    }

    public async Task<bool> RemoveSession(string sessionId)
    {
        return await _database.KeyDeleteAsync(SessionSetKey(sessionId));
    }

    private string UserSetkey(string userId) => $"user:{userId}";

    private string SessionSetKey(string sessionId) => $"session:{sessionId}";
    
    private string SessionToUserIdSetKey(string sessionId) => $"sessionUserId:{sessionId}";
    private string ConnectionIdKey(string connection) => $"connectionId:{connection}";
}