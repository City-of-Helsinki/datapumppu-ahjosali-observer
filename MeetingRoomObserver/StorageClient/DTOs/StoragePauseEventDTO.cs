namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StoragePauseEventDTO: StorageEventDTO
    {
        public StoragePauseEventDTO()
        {
            EventType = StorageEventType.Pause;
        }
    }
}
