namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO for speech timer updates, containing speaker, seat, duration, timer value, direction, and additional info.
    /// </summary>
    public class StorageSpeechTimerEventDTO : StorageEventDTO
    {
        public StorageSpeechTimerEventDTO()
        {
            EventType = StorageEventType.SpeechTimer;
        }

        public string SeatID { get; set; } = string.Empty;

        public string Person { get; set; } = string.Empty;

        public int DurationSeconds { get; set; }

        public int SpeechTimer { get; set; }

        public string Direction { get; set; }

        public string AdditionalInfoFI { get; set; }

        public string AdditionalInfoSV { get; set; }
    }
}
