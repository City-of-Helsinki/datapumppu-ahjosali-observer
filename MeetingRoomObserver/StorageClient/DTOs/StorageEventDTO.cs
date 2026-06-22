namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Base storage event DTO containing common fields: meeting ID, event type, timestamp, sequence number, case number, and item number.
    /// </summary>
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
