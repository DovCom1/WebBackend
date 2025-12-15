using System.Security.Cryptography;
using WebBackend.Model.Manager;

namespace WebBackend.Service.Manager;

public class GeneratorManager : IGeneratorManager
{
    public string GenerateSid()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }
}