namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Represents a seat assignment for storage, mapping a person to a seat ID with additional info in Finnish and Swedish.
    /// </summary>
    public class StorageMeetingSeatDTO
    {
        public string SeatID { get; set; } = string.Empty;

        public string? Person { get; set; }

        public string? AdditionalInfoFI { get; set; }

        public string? AdditionalInfoSV { get; set; }
    }
}
