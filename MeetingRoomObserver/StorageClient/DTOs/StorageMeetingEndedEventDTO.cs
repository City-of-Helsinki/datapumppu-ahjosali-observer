namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO indicating that the meeting has ended.
    /// </summary>
    public class StorageMeetingEndedEventDTO : StorageEventDTO
    {
        public StorageMeetingEndedEventDTO()
        {
            EventType = StorageClient.StorageEventType.MeetingEnded;
        }
    }
}
