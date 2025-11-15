using WebBackend.Model.Request;

namespace WebBackend.Model.Manager
{
    public interface IConductorManager
    {
        Task<ConductorResponse> SendProxyRequestAsync(HttpMethod method, string service, string endpoint, object? data = null);
        Task<ConductorResponse> SendSignalRequestAsync(object data);
    }
}
