using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebBackend.Model.Storage;

namespace WebBackend.Api.Hubs;

[Authorize(AuthenticationSchemes = "TokenScheme")]
public class UserHub : Hub
{
    private readonly ILogger<UserHub> _logger;
    private readonly ISessionStorage _sessionStorage;

    public UserHub(ILogger<UserHub> logger, ISessionStorage storage)
    {
        _logger = logger;
        _sessionStorage = storage;
    }
    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("User connected by ConnectionId: {ConnectionId}", Context.ConnectionId);
        await _sessionStorage.AddConnection(Context.ConnectionId);
        await base.OnConnectedAsync();
    }
    public async Task Connect(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"{userId}");
        _logger.LogInformation($"Connection {Context.ConnectionId} joined {userId}");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation($"User with connectionId {Context.ConnectionId} disconnected");
        await _sessionStorage.RemoveConnection(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}