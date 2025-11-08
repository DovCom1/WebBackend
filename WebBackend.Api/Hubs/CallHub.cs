using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebBackend.Model.Dto.Calls;
using WebBackend.Model.Manager;

namespace WebBackend.Api.Hubs
{
    [Authorize(AuthenticationSchemes = "TokenScheme")]
    public class CallHub : Hub
    {
        private readonly IConductorManager _conductorManager;
        private readonly ILogger<CallHub> _logger;

        public CallHub(IConductorManager conductorManager, ILogger<CallHub> logger)
        {
            _conductorManager = conductorManager;
            _logger = logger;
        }

        public async Task Connect(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{userId}");
            _logger.LogInformation($"Connection {Context.ConnectionId} joined {userId}");
        }

        public async Task SendCallOffer(SignalDto offer)
        {
            if (string.IsNullOrEmpty(offer.ToUserId) || string.IsNullOrEmpty(offer.FromUserId))
                throw new HubException("Invalid offer data");

            var responce = await _conductorManager.SendSignalRequestAsync(offer);

            _logger.LogInformation($"Offer from {offer.FromUserId} to {offer.ToUserId} forwarded to Conductor");

            if (!responce.IsSuccess)
                _logger.LogWarning($"Responce from Conductor is not successfull");
        }

        public async Task SendCallAnswer(SignalDto answer)
        {
            if (string.IsNullOrEmpty(answer.ToUserId) || string.IsNullOrEmpty(answer.FromUserId))
                throw new HubException("Invalid answer data");

            await Clients.Group($"{answer.ToUserId}")
                .SendAsync("ReceiveCallAnswer", answer);

            _logger.LogInformation($"Answer from {answer.FromUserId} sent to {answer.ToUserId} via Hub");
        }

        public async Task SendIceCandidate(CallIceCandidateDto candidate)
        {
            if (string.IsNullOrEmpty(candidate.ToUserId) || string.IsNullOrEmpty(candidate.FromUserId))
                throw new HubException("Invalid ICE candidate data");

            await Clients.Group($"{candidate.ToUserId}")
                .SendAsync("ReceiveIceCandidate", candidate);

            _logger.LogInformation($"ICE candidate from {candidate.FromUserId} sent to {candidate.ToUserId}");
        }
    }
}