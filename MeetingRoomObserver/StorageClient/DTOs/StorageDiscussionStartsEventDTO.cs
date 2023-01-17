namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageDiscussionStartsEventDTO : StorageEventDTO
    {
        public StorageDiscussionStartsEventDTO()
        {
            EventType = StorageEventType.DiscussionStarts;
        }
    }
}
