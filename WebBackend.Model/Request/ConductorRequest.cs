namespace WebBackend.Model.Request;

public class ConductorRequest
{
    public required HttpMethod Method { get; set; }
    public required string Service { get; set; }
    public required string Endpoint { get; set; }
    public object? Data { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();
}

public class ConductorResponse
{
    public bool IsSuccess { get; set; }
    public string Content { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();
}