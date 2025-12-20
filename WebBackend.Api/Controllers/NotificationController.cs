using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Логируем все ошибки
                var errors = ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new {
                        Field = e.Key,
                        Errors = e.Value.Errors.Select(err => err.ErrorMessage)
                    });
        
                logger.LogError("Validation failed: {@Errors}", errors);
                return BadRequest(ModelState);
            }
            await hubContext.Clients.Group(dto.ReceiverId.ToString())
                .SendAsync("ReceiveNotification", dto);

            return Ok();
        }
    }
}
