namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageStatementEndedEventDTO: StorageEventDTO
    {
        public StorageStatementEndedEventDTO()
        {
            EventType = StorageEventType.StatementEnded;
        }
    }
}
