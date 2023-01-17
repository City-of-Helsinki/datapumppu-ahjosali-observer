namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageMeetingStartedEventDTO : StorageEventDTO
    {
        public StorageMeetingStartedEventDTO()
        {
            EventType = StorageEventType.MeetingStarted;
        }

        public string MeetingTitleFI { get; set; }

        public string MeetingTitleSV { get; set; }
    }
}
