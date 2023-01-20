namespace MeetingRoomObserver.StorageClient.DTOs
{
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
