using WebBackend.Model.Service;
using System.Security.Cryptography;

namespace WebBackend.Service.Service;

public class GeneratorService : IGeneratorService
{
    public string GenerateWebToken()
    {
        return GenerateSid();
    }

    public string GenerateSid()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }
}