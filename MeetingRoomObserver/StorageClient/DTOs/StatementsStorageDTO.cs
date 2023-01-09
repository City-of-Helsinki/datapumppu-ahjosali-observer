namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StatementsStorageDTO : StorageEventDTO
    {
        public List<StatementStorageDTO> SpeakingTurns { get; set; }
    }
}
