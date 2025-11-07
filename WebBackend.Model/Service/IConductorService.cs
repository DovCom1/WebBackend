using WebBackend.Model.Request;

namespace WebBackend.Model.Service;

public interface IConductorService
{
    Task<ConductorResponse> SendAsync(ConductorRequest request);
}