namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO for a statement reservation (speaking turn request), with person, ordinal, seat, and additional info.
    /// </summary>
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
