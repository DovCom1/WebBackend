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

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("User connected by ConnectionId: {ConnectionId}", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public async Task Connect(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{userId}");
            _logger.LogInformation($"Connection {Context.ConnectionId} joined {userId}");
        }

        public async Task SendCallSignal(SignalingMessageDto signalDto)
        {
            var responce = await _conductorManager.SendSignalRequestAsync(signalDto);

            _logger.LogInformation($"Message from {signalDto.From} to {signalDto.To} forwarded to Conductor");

            if (!responce.IsSuccess)
                _logger.LogWarning($"Responce from Conductor is not successfull");
        }
    }
}