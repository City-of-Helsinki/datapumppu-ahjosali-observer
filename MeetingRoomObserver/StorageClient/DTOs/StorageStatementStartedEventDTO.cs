namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO indicating that a statement (speech) has started, with speaker, timing, direction, seat, and speech type.
    /// </summary>
    public class StorageStatementStartedEventDTO : StorageEventDTO
    {
        public StorageStatementStartedEventDTO()
        {
            EventType = StorageEventType.StatementStarted;
        }

        public string? Person { get; set; }

        public int? SpeakingTime { get; set; }

        public int? SpeechTimer { get; set; }

        public DateTime? StartTime { get; set; }

        public string? Direction { get; set; }

        public string? SeatID { get; set; }

        public StorageSpeechType? SpeechType { get; set; }

        public string? AdditionalInfoFI { get; set; }

        public string? AdditionalInfoSV { get; set; }
    }
}
