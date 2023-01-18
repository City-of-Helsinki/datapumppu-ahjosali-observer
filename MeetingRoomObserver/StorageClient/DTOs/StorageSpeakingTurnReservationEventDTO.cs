namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageSpeakingTurnReservationEventDTO : StorageEventDTO
    {
        public StorageSpeakingTurnReservationEventDTO()
        {
            EventType = StorageEventType.SpeakingTurnReservation;
        }

        public string? Person { get; set; }

        public int? Ordinal { get; set; }

        public string? SeatID { get; set; }

        public string? AdditionalInfoFI { get; set; }

        public string? AdditionalInfoSV { get; set; }
    }
}
