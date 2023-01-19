namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageReplyReservationsClearedEventDTO: StorageEventDTO
    {
        public StorageReplyReservationsClearedEventDTO()
        {
            EventType = StorageEventType.ReplyReservationsCleared;
        }
    }
}
