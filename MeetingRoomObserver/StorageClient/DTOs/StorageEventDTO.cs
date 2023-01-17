namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageEventDTO
    {
        public string MeetingID { get; set; } = string.Empty;

        public StorageEventType EventType { get; set; }

        public DateTime Timestamp { get; set; }

        public long SequenceNumber { get; set; }

        public string CaseNumber { get; set; } = string.Empty;

        public string ItemNumber { get; set; } = string.Empty;
    }
}
