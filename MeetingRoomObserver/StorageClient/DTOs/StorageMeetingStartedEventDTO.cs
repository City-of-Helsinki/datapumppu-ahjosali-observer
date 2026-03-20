namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO indicating that the meeting has started, containing the meeting title in Finnish and Swedish.
    /// </summary>
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
