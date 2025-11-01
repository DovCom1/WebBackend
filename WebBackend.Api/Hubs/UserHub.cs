using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebBackend.Api.Hubs;

[Authorize(AuthenticationSchemes = "TokenScheme")]
public class UserHub : Hub
{
    
}