namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageMeetingContinuesEventDTO : StorageEventDTO
    {
        public StorageMeetingContinuesEventDTO()
        {
            EventType = StorageEventType.MeetingContinues;
        }
    }
}
