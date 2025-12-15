namespace WebBackend.Model.Configuration;

public class RedisConnection
{
    public required string ConnectionString { get; init; }
    public required string Password { get; init; }
}