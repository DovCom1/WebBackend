namespace WebBackend.Model.Dto.Calls
{
    public record CallIceCandidateDto(
        string CallId,
        string FromUserId,
        string ToUserId,
        string Candidate,
        string SdpMid,
        int SdpMLineIndex
    );
}
