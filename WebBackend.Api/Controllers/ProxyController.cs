using Microsoft.AspNetCore.Mvc;
using WebBackend.Model.Request;
using WebBackend.Model.Service;

namespace WebBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        return await HandleProxyRequest(HttpMethod.Post, service, $"{endpoint}{Request.QueryString}", data);
    }

    [HttpGet("{service}/{*endpoint}")]
    public async Task<IActionResult> GetProxy(string service, string endpoint)
    {
        return await HandleProxyRequest(HttpMethod.Get, service, $"{endpoint}{Request.QueryString}");
    }

    [HttpPut("{service}/{*endpoint}")]
    public async Task<IActionResult> PutProxy(string service, string endpoint, [FromBody] object data)
    {
        return await HandleProxyRequest(HttpMethod.Put, service, $"{endpoint}{Request.QueryString}", data);
    }

    [HttpDelete("{service}/{*endpoint}")]
    public async Task<IActionResult> DeleteProxy(string service, string endpoint)
    {
        return await HandleProxyRequest(HttpMethod.Delete, service, $"{endpoint}{Request.QueryString}");
    }

    [HttpPatch("{service}/{*endpoint}")]
    public async Task<IActionResult> PatchProxy(string service, string endpoint, [FromBody] object data)
    {
        return await HandleProxyRequest(HttpMethod.Patch, service, $"{endpoint}{Request.QueryString}", data);
    }

    private async Task<IActionResult> HandleProxyRequest(HttpMethod method, string service, string endpoint, object? data = null)
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
}