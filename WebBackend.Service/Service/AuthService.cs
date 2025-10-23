using System.Text.Json;
using WebBackend.Model.Dto;
using WebBackend.Model.Request;
using WebBackend.Model.Service;

namespace WebBackend.Service.Service;

public class AuthService(IHttpClientFactory clientFactory, RequestFactory requestFactory) : IAuthService
{
    private readonly IHttpClientFactory _clientFactory = clientFactory;
    private readonly RequestFactory _requestFactory = requestFactory;
    public async Task<AuthTokenDto?> LoginAsync(AuthenticateDto dto)
    {
        var client = _clientFactory.CreateClient("AuthService");
        var response = await client.SendAsync(_requestFactory.CreateLoginRequest(dto));
        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<AuthTokenDto>(await response.Content.ReadAsStringAsync());
        }
        return null;
    }

    public async Task<bool> RegisterAsync(AuthenticateDto dto)
    {
        var client = _clientFactory.CreateClient("AuthService");
        var response = await client.SendAsync(_requestFactory.CreateRegisterRequest(dto));
        return response.IsSuccessStatusCode;
    }
}