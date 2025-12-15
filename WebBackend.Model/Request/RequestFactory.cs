using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WebBackend.Model.Configuration;
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

    public HttpRequestMessage CreateHttpRequestAsync(ConductorRequest request)
    {
        var url = $"{_requestDomains.ConductorService}/api/{request.Service}/{request.Endpoint.TrimStart('/')}";
        var httpRequest = new HttpRequestMessage(request.Method, url);

        if (request.Data is not null)
        {
            var json = JsonSerializer.Serialize(request.Data);
            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        foreach (var header in request.Headers)
        {
            httpRequest.Headers.Add(header.Key, header.Value);
        }

        return httpRequest;
    }
}