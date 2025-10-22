namespace WebBackend.Model.Service;

public interface IGeneratorService
{
    string GenerateWebToken();

    string GenerateSid();
}