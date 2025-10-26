using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WebBackend.Model.Dto;

namespace WebBackend.Model.Request;

public class RequestFactory(IOptions<RequestDomains> options)
{
    private readonly RequestDomains _requestDomains = options.Value;

    public HttpRequestMessage CreateLoginRequest(AuthenticateDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        return new HttpRequestMessage(
            HttpMethod.Post,
            _requestDomains.AuthService + RequestPath.LoginUrl)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }
    

    public HttpRequestMessage CreateRegisterRequest(AuthenticateDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        Console.WriteLine("auth service is " + _requestDomains.AuthService);
        return new HttpRequestMessage(
            HttpMethod.Post,
            _requestDomains.AuthService + RequestPath.RegisterUrl)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }
}