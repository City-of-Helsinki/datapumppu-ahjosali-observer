namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO for a reply reservation (vastauspuheenvuorovaraus), with person, ordinal, seat, and additional info.
    /// </summary>
    public class StorageReplyReservationEventDTO: StorageEventDTO
    {
        public StorageReplyReservationEventDTO()
        {
            EventType = StorageEventType.ReplyReservation;
        }

        public string? Person { get; set; }

        public string? AdditionalInfoFI { get; set; }

        public string? AdditionalInfoSV { get; set; }

        public int? Ordinal { get; set; }

        public string? SeatID { get; set; }
    }
}
