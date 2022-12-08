namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class MeetingEndedStorageDTO : StorageEventDTO
    {
        public MeetingEndedStorageDTO()
        {
            EventType = StorageClient.StorageEventType.MeetingEnded;
        }
    }
}
