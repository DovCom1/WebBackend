using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using WebBackend.Api.Hubs;

namespace WebBackend.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController(
    ILogger<AuthController> logger,
    IHubContext<UserHub> hubContext) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] Dictionary<string, string> dto)
        {
            logger.LogInformation($"DTO IS {dto.ToString()}");
            var type = dto["TypeDto"]?.ToString();
            var receiverId = dto["ReceiverId"]?.ToString();
            if (type == null || receiverId == null)
            {
                logger.LogError($"Invalid notification type: {type}");
                logger.LogError($"Invalid receiver id: {receiverId}");
                return BadRequest("Invalid notification type or receiver id.");
            }
            logger.LogInformation($"Get type: {type}");
            logger.LogInformation($"Get receiver id: {receiverId}");

            if (type == "SendMessage")
            {
                logger.LogInformation($"Sending notification to new message");
                await hubContext.Clients.Group(receiverId)
                    .SendAsync("ReceiveNotification", dto);
            }
            else if (type == "Invite")
            {
                logger.LogInformation($"Sending notification to new invite");
                await hubContext.Clients.Group(receiverId)
                    .SendAsync("Invite", dto);
            }
            return Ok();
        }
    }
}
