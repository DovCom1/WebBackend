namespace WebBackend.Model.Configs;

public class CorsConfig
{
    public string[] AllowedOrigins { get; set; } = new[] { "http://localhost:3000" };
    public string[] AllowedMethods { get; set; } = new[] { "GET", "POST", "PUT", "DELETE", "PATCH" };
}