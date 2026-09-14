namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO indicating that the meeting has resumed after a pause.
    /// </summary>
    public class StorageMeetingContinuesEventDTO : StorageEventDTO
    {
        public StorageMeetingContinuesEventDTO()
        {
            EventType = StorageEventType.MeetingContinues;
        }
    }
}
