namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageReplyReservationEventDTO: StorageEventDTO
    {
        public StorageReplyReservationEventDTO()
        {
            EventType = StorageEventType.ReplyReservation;
        }

        public string? PersonFI { get; set; }

        public string? PersonSV { get; set; }
    }
}
