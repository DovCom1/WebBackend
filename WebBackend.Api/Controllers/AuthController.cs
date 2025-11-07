using Microsoft.AspNetCore.Mvc;
using WebBackend.Api.Service;
using WebBackend.Model.Dto;
using WebBackend.Model.Manager;
 

namespace WebBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    ILogger<AuthController> logger,
    IAuthManager authManager) : ControllerBase
{
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var sid = await authManager.TryAuthenticate(dto);
        CookieCreator.AddSidToCookie(sid, Response);
        return Ok();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (await authManager.TryRegister(dto))
        {
            return Created();
        }
        return BadRequest();
    }
}