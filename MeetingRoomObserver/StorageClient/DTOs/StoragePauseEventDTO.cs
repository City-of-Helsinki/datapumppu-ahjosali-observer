namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO indicating that the meeting has been paused.
    /// </summary>
    public class StoragePauseEventDTO: StorageEventDTO
    {
        public StoragePauseEventDTO()
        {
            EventType = StorageEventType.Pause;
        }
    }
}
