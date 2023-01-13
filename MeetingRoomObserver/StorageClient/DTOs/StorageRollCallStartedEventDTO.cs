namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageRollCallStartedEventDTO: StorageEventDTO
    {
        public StorageRollCallStartedEventDTO()
        {
            EventType = StorageEventType.RollCallStarted;
        }
    }
}
