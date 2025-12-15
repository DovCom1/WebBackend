namespace WebBackend.Model.Dto.Calls
{
    public record RoomInfoDto(Guid RoomId, string Name, List<Guid> Participants);
}
