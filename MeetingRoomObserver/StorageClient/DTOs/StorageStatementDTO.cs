namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Represents an individual statement (speech record) for storage, with person, timing, speech type, duration, and additional info.
    /// </summary>
    public class StorageStatementDTO
    {
        public string? Person { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public StorageSpeechType? SpeechType { get; set; }

        public int? Duration { get; set; }

        public string? AdditionalInfoFI { get; set; }

        public string? AdditionalInfoSV { get; set; }
    }
}
