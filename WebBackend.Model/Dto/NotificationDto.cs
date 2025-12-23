namespace WebBackend.Model.Dto
{
    public record NotificationDto(
        Guid SenderId,
        Guid ReceiverId,
        Guid ChatId,
        string Message,
        string SenderName,
        string ReceiverName,
        string ChatName,
        string CreatedAt);
}
