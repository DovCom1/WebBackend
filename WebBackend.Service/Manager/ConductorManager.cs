using Microsoft.Extensions.Logging;
using WebBackend.Model.Constants;
using WebBackend.Model.Manager;
using WebBackend.Model.Request;
using WebBackend.Model.Service;

namespace WebBackend.Service.Manager
{
    public class ConductorManager(
        ILogger<AuthManager> logger,
        IConductorService conductorService) : IConductorManager
    {
        public async Task<ConductorResponse> SendProxyRequestAsync(HttpMethod method, string service, string endpoint, object? data = null)
        {
            var request = new ConductorRequest
            {
                Method = method,
                Service = service,
                Endpoint = endpoint,
                Data = data
            };

            return await conductorService.SendAsync(request);
        }

        public async Task<ConductorResponse> SendSignalRequestAsync(object data)
        {
            var request = new ConductorRequest
            {
                Method = HttpMethod.Post,
                Service = Constants.CallsServiceName,
                Endpoint = RequestPath.CallSignalUrl,
                Data = data
            };

            return await conductorService.SendAsync(request);
        }
    }
}
