namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageBreakNoticeEventDTO: StorageEventDTO
    {
        public string Notice { get; set; }

        public StorageBreakNoticeEventDTO()
        {
            EventType = StorageEventType.BreakNotice;
        }
    }
}
