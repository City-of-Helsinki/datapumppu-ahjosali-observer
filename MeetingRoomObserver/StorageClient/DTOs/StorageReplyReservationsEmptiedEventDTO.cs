namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageReplyReservationsEmptiedEventDTO: StorageEventDTO
    {
        public StorageReplyReservationsEmptiedEventDTO()
        {
            EventType = StorageEventType.ReplyReservationsEmptied;
        }
    }
}
