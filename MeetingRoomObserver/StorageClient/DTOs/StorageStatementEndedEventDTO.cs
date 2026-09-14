namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO indicating that a statement (speech) has ended.
    /// </summary>
    public class StorageStatementEndedEventDTO: StorageEventDTO
    {
        public StorageStatementEndedEventDTO()
        {
            EventType = StorageEventType.StatementEnded;
        }
    }
}
