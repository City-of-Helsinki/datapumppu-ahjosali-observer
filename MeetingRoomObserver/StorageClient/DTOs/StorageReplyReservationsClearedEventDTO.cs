namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO indicating that all reply reservations have been cleared.
    /// </summary>
    public class StorageReplyReservationsClearedEventDTO: StorageEventDTO
    {
        public StorageReplyReservationsClearedEventDTO()
        {
            EventType = StorageEventType.ReplyReservationsCleared;
        }
    }
}
