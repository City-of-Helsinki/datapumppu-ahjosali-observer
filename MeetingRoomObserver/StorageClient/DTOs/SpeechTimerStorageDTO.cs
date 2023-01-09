namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class SpeechTimerStorageDTO : StorageEventDTO
    {
        public string SeatID { get; set; } = string.Empty;

        public string Person { get; set; } = string.Empty;

        public string AdditionalInfoFI { get; set; } = string.Empty;

        public string AdditionalInfoSV { get; set; } = string.Empty;

        public int DurationSeconds { get; set; }

        public int SpeechTimer { get; set; }

        public string Direction { get; set; } = string.Empty;

    }
}
