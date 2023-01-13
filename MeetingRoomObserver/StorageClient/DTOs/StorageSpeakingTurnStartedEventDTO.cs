namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageSpeakingTurnStartedEventDTO : StorageEventDTO
    {
        public StorageSpeakingTurnStartedEventDTO()
        {
            EventType = StorageEventType.SpeakingTurnStarted;
        }

        public string? PersonFI { get; set; }

        public string? PersonSV { get; set; }

        public int? SpeakingTime { get; set; }

        public int? SpeechTimer { get; set; }

        public DateTime? StartTime { get; set; }

        public string? Direction { get; set; }

        public string? SeatID { get; set; }

        public StorageSpeechType? SpeechType { get; set; }
    }
}
