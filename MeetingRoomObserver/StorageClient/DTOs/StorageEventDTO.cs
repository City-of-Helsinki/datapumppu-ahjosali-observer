using MeetingRoomObserver.Models;

namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageEventDTO
    {
        public string MeetingID { get; set; } = string.Empty;

        public long SequenceNumber { get; set; }

        public StorageEventType EventType { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
