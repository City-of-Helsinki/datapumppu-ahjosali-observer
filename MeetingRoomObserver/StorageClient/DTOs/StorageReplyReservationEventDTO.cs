namespace MeetingRoomObserver.StorageClient.DTOs
{
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
