namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO indicating that a person has arrived at the meeting, with their name, seat, and additional info in Finnish and Swedish.
    /// </summary>
    public class StoragePersonArrivedEventDTO : StorageEventDTO
    {
        public StoragePersonArrivedEventDTO()
        {
            EventType = StorageEventType.PersonArrived;
        }

        public string? Person { get; set; }

        public string? SeatID { get; set; }

        public string? AdditionalInfoFI { get; set; }

        public string? AdditionalInfoSV { get; set; }
    }
}
