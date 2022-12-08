namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class MeetingStartedStorageDTO : StorageEventDTO
    {
        public MeetingStartedStorageDTO()
        {
            EventType = StorageClient.StorageEventType.MeetingStarted;
        }

        public string MeetingTitleFI { get; set; }

        public string MeetingTitleSV { get; set; }


    }
}
