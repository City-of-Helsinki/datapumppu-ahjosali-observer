namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageSpeechTimerEventDTO : StorageEventDTO
    {
        public StorageSpeechTimerEventDTO()
        {
            EventType = StorageEventType.SpeechTimer;
        }

        public string SeatID { get; set; } = string.Empty;

        public string PersonFI { get; set; } = string.Empty;

        public string PersonSV { get; set; } = string.Empty;

        public int DurationSeconds { get; set; }

        public int SpeechTimer { get; set; }

        public string Direction { get; set; }
    }
}
