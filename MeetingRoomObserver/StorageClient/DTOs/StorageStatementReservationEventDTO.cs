namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageStatementReservationEventDTO : StorageEventDTO
    {
        public StorageStatementReservationEventDTO()
        {
            EventType = StorageEventType.StatementReservation;
        }

        public string? Person { get; set; }

        public int? Ordinal { get; set; }

        public string? SeatID { get; set; }

        public string? AdditionalInfoFI { get; set; }

        public string? AdditionalInfoSV { get; set; }
    }
}
