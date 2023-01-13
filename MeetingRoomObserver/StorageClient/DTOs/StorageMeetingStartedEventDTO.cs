namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageMeetingStartedEventDTO : StorageEventDTO
    {
        public StorageMeetingStartedEventDTO()
        {
            EventType = StorageClient.StorageEventType.MeetingStarted;
        }

        public string MeetingTitleFI { get; set; }

        public string MeetingTitleSV { get; set; }


    }
}
