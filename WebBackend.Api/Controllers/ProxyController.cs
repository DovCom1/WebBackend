using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBackend.Model.Request;
using WebBackend.Model.Service;

namespace WebBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class ProxyController : ControllerBase
{
    private readonly IConductorService _conductorService;

    public ProxyController(IConductorService conductorService)
    {
        _conductorService = conductorService;
    }

    [HttpPost("{service}/{*endpoint}")]
    public async Task<IActionResult> PostProxy(string service, string endpoint, [FromBody] object data)
    {
        return await HandleProxyRequest("POST", service, endpoint, data);
    }

    [HttpGet("{service}/{*endpoint}")]
    public async Task<IActionResult> GetProxy(string service, string endpoint)
    {
        return await HandleProxyRequest("GET", service, endpoint);
    }

    [HttpPut("{service}/{*endpoint}")]
    public async Task<IActionResult> PutProxy(string service, string endpoint, [FromBody] object data)
    {
        return await HandleProxyRequest("PUT", service, endpoint, data);
    }

    [HttpDelete("{service}/{*endpoint}")]
    public async Task<IActionResult> DeleteProxy(string service, string endpoint)
    {
        return await HandleProxyRequest("DELETE", service, endpoint);
    }

    [HttpPatch("{service}/{*endpoint}")]
    public async Task<IActionResult> PatchProxy(string service, string endpoint, [FromBody] object data)
    {
        return await HandleProxyRequest("PATCH", service, endpoint, data);
    }

    private async Task<IActionResult> HandleProxyRequest(string method, string service, string endpoint, object? data = null)
    {
        try
        {
            var request = new ConductorRequest
            {
                Method = method,
                Service = service,
                Endpoint = endpoint,
                Data = data
            };

            var response = await _conductorService.SendAsync(request);

            return response.IsSuccess
                ? Content(response.Content, "application/json")
                : StatusCode(response.StatusCode, response.Content);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Proxy error: {ex.Message}");
        }
    }
}