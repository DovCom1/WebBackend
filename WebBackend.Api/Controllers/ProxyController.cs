using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebBackend.Model.Manager;

namespace WebBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "TokenScheme")]
public class ProxyController : ControllerBase
{
    private readonly IConductorManager _conductorManager;

    public ProxyController(IConductorManager conductorManager)
    {
        _conductorManager = conductorManager;
    }

    [HttpPost("{service}/{*endpoint}")]
    public async Task<IActionResult> PostProxy(string service, string endpoint, [FromBody] object? data)
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
    public async Task<IActionResult> PatchProxy(
        [FromRoute] string service, 
        [FromRoute] string endpoint, 
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] object? data = null)
    {
        return await HandleProxyRequest(HttpMethod.Patch, service, $"{endpoint}{Request.QueryString}", data);
    }

    private async Task<IActionResult> HandleProxyRequest(HttpMethod method, string service, string endpoint, object? data = null)
    {
        var response = await _conductorManager.SendProxyRequestAsync(method, service, endpoint, data);

        return response.IsSuccess
            ? Content(response.Content, "application/json")
            : StatusCode(response.StatusCode, response.Content);
    }
}