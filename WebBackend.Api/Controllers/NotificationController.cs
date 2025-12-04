using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebBackend.Api.Hubs;
using WebBackend.Model.Dto;

namespace WebBackend.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController(
    ILogger<AuthController> logger,
    IHubContext<UserHub> hubContext) : ControllerBase
    {
        [HttpPost("notification/send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationDto dto)
        {
            await hubContext.Clients.Group(dto.chatId)
                .SendAsync("ReceiveNotification", dto);

            return Ok();
        }
    }
}
