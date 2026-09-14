namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO indicating that discussion has started on a case.
    /// </summary>
    public class StorageDiscussionStartsEventDTO : StorageEventDTO
    {
        public StorageDiscussionStartsEventDTO()
        {
            EventType = StorageEventType.DiscussionStarts;
        }
    }
}
