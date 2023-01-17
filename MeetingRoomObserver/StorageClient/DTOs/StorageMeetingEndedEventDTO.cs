namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageMeetingEndedEventDTO : StorageEventDTO
    {
        public StorageMeetingEndedEventDTO()
        {
            EventType = StorageClient.StorageEventType.MeetingEnded;
        }
    }
}
