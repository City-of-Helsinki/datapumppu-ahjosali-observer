namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageSpeakingTurnReservationEventDTO : StorageEventDTO
    {
        public StorageSpeakingTurnReservationEventDTO()
        {
            EventType = StorageEventType.SpeakingTurnReservation;
        }

        public string? PersonFI { get; set; }

        public string? PersonSV { get; set; }

        public int? Ordinal { get; set; }

        public string? SeatID { get; set; }
    }
}
