namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO indicating that a roll call has started.
    /// </summary>
    public class StorageRollCallStartedEventDTO: StorageEventDTO
    {
        public StorageRollCallStartedEventDTO()
        {
            EventType = StorageEventType.RollCallStarted;
        }
    }
}
