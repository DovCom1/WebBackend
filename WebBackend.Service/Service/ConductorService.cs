using Microsoft.Extensions.Logging;
using WebBackend.Model.Request;
using WebBackend.Model.Service;

namespace WebBackend.Service.Service;

public class ConductorService(
    IHttpClientFactory clientFactory,
    RequestFactory requestFactory,
    ILogger<AuthService> logger) : IConductorService
{
    public async Task<ConductorResponse> SendAsync(ConductorRequest request)
    {
        try
        {
            var httpClient = clientFactory.CreateClient();
            var response = await httpClient.SendAsync(requestFactory.CreateHttpRequestAsync(request));
            
            return await CreateConductorResponse(response);
        }
        catch (Exception ex)
        {
            logger.LogError($"Exception occured: {ex.Message}");
            return new ConductorResponse
            {
                IsSuccess = false,
                StatusCode = 500,
                Content = $"{{\"error\": \"Conductor service error: {ex.Message}\"}}"
            };
        }
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