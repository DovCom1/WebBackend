using Microsoft.AspNetCore.Mvc;
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
        
        var status = await authManager.TryAuthenticate();
        if (!status)
        {
            return BadRequest();
        }

        return Created();
    }

    // [HttpPost("register")]
    // public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    // {
    //     
    // }
}