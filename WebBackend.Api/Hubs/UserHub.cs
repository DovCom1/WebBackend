using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebBackend.Api.Hubs;

[Authorize(AuthenticationSchemes = "TokenScheme")]
public class UserHub : Hub
{
    private readonly ILogger<UserHub> _logger;

    public UserHub(ILogger<UserHub> logger)
    {
        _logger = logger;
    }
    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("User connected by ConnectionId: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }
    public async Task Connect(int chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"{chatId}");
        _logger.LogInformation($"Connection {Context.ConnectionId} joined {chatId}");
    }
}