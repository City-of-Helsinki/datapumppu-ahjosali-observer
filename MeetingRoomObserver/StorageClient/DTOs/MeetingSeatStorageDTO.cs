namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class MeetingSeatStorageDTO
    {
        public string SeatID { get; set; } = string.Empty;

        public string? Person { get; set; }

        public string? AdditionalInfoFI { get; set; }

        public string? AdditionalInfoSV { get; set; }
    }
}
