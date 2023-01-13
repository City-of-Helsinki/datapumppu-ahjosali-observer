namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageBreakEventDTO: StorageEventDTO
    {
        public StorageBreakEventDTO()
        {
            EventType = StorageEventType.Break;
        }
    }
}
