namespace WebBackend.Model.Dto.Calls
{
    public record SignalDto(
        string CallId,
        string FromUserId,
        string ToUserId,
        string Sdp,
        DateTime Timestamp)
    {
        public SignalDto(string callId, string fromUserId, string toUserId, string sdp)
            : this(callId, fromUserId, toUserId, sdp, DateTime.UtcNow)
        {
        }
    }
}
