using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WebBackend.Model.Configs;
using WebBackend.Model.Request;
using WebBackend.Model.Service;

namespace WebBackend.Service.Service;

public class ConductorService : IConductorService
{
    private readonly HttpClient _httpClient;
    private readonly ConductorConfig _config;
    private readonly JsonSerializerOptions _jsonOptions;

    public ConductorService(HttpClient httpClient, IOptions<ConductorConfig> config)
    {
        _httpClient = httpClient;
        _config = config.Value;
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task<ConductorResponse> SendAsync(ConductorRequest request)
    {
        try
        {
            var httpRequest = CreateHttpRequest(request);
            var response = await _httpClient.SendAsync(httpRequest);
            
            return await CreateConductorResponse(response);
        }
        catch (Exception ex)
        {
            return new ConductorResponse
            {
                IsSuccess = false,
                StatusCode = 500,
                Content = $"{{\"error\": \"Conductor service error: {ex.Message}\"}}"
            };
        }
    }

    private HttpRequestMessage CreateHttpRequest(ConductorRequest request)
    {
        var url = $"{_config.BaseUrl}/api/{request.Service}/{request.Endpoint.TrimStart('/')}";
        var httpRequest = new HttpRequestMessage(request.Method, url);
        
        if (request.Data is not null)
        {
            var json = JsonSerializer.Serialize(request.Data, _jsonOptions);
            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }
        
        foreach (var header in request.Headers)
        {
            httpRequest.Headers.Add(header.Key, header.Value);
        }
        
        return httpRequest;
    }

    private async Task<ConductorResponse> CreateConductorResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        
        var conductorResponse = new ConductorResponse
        {
            IsSuccess = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            Content = content
        };

        return conductorResponse;
    }
}