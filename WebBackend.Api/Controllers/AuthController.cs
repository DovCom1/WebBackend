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
        
        var sid = await _authManager.TryAuthenticate(dto);
        CookieCreator.AddSidToCookie(sid, Response);

        var webToken = _authManager.CreateWebToken();
        
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
}