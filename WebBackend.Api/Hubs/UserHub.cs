using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebBackend.Model.Dto;

namespace WebBackend.Api.Hubs;

[Authorize(AuthenticationSchemes = "TokenScheme")]
public class UserHub : Hub
{
    public async Task Connect(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"{userId}");
        Console.WriteLine($"Connection {Context.ConnectionId} joined {userId}");
    }

    public async Task SendNotification(string userId, NotificationDto dto)
    {
        await Clients.Group($"{userId}")
            .SendAsync("ReceiveNotification", dto);
    }
}