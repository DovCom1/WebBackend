using System.Text.Json;
using Microsoft.Extensions.Logging;
using WebBackend.Model.Dto;
using WebBackend.Model.Request;
using WebBackend.Model.Service;

namespace WebBackend.Service.Service;

public class AuthService(IHttpClientFactory clientFactory, RequestFactory requestFactory, ILogger<AuthService> logger) : IAuthService
{
    public async Task<AuthTokenDto?> LoginAsync(AuthenticateDto dto)
    {
        var client = clientFactory.CreateClient();
        var request = requestFactory.CreateLoginRequest(dto);
        var response = await client.SendAsync(request);
        
        if (response.IsSuccessStatusCode)
        {
            var authTokenDto = JsonSerializer.Deserialize<AuthTokenDto>(
                await response.Content.ReadAsStringAsync(), 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (authTokenDto == null)
            {
                logger.LogError("AuthTokenDto is null");
                return null;
            }
            logger.LogInformation("AuthTokenDto is not null");
            return authTokenDto;
        }
        logger.LogError("Bad status code");
        return null;
    }

    public async Task<bool> RegisterAsync(AuthenticateDto dto)
    {
        var client = clientFactory.CreateClient();
        var response = await client.SendAsync(requestFactory.CreateRegisterRequest(dto));
        return response.IsSuccessStatusCode;
    }
}