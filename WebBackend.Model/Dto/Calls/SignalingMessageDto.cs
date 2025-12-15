namespace WebBackend.Model.Dto.Calls
{    
    public record SignalingMessageDto(
        SignalingMessageType Type,
        Guid From,
        Guid To, 
        PayloadDto Payload);
}