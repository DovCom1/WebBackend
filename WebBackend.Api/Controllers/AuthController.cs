using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WebBackend.Api.Service;
using WebBackend.Model.Dto;
using WebBackend.Model.Manager;
 

namespace WebBackend.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(
    ILogger<AuthController> logger,
    IAuthManager authManager) : ControllerBase
{
    private readonly IAuthManager _authManager =  authManager;
    private readonly ILogger<AuthController> _logger = logger;
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        
        var (sid, webToken) = await _authManager.TryAuthenticate(dto);
        CookieCreator.AddSidToCookie(sid, Response);
        
        
        return Ok(new WebTokenDto(webToken));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (await _authManager.TryRegister(dto))
        {
            return Created();
        }
        return BadRequest();
    }

    // [HttpGet("connect")]
    // public async Task CreateWebSocket([FromQuery] string token)
    // {
    //     _logger.LogInformation("Connect websocket to token");
    //     if (!HttpContext.WebSockets.IsWebSocketRequest && !_authManager.VerifyToken(token))
    //     {
    //         _logger.LogWarning("Failed to connect the socket");
    //         HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
    //         return;
    //     }
    //     _logger.LogInformation("Token is valid; request is websocket request");
    //
    //     using var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
    //
    //     var buffer = new byte[4096];
    //     while (true)
    //     {
    //         var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
    //
    //         // Если клиент закрыл соединение
    //         if (result.MessageType == WebSocketMessageType.Close)
    //         {
    //             await socket.CloseAsync(
    //                 result.CloseStatus ?? WebSocketCloseStatus.NormalClosure,
    //                 result.CloseStatusDescription,
    //                 CancellationToken.None);
    //             break;
    //         }
    //
    //         // Собираем текст (в случае если фрагментировано)
    //         var text = Encoding.UTF8.GetString(buffer, 0, result.Count);
    //         var replyText = $"echo: {text}";
    //         var replyBytes = Encoding.UTF8.GetBytes(replyText);
    //
    //         await socket.SendAsync(replyBytes, WebSocketMessageType.Text, true, CancellationToken.None);
    //     }
    // }
}