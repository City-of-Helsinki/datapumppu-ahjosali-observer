namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageSpeakingTurnReservationsEmptiedEventDTO: StorageEventDTO
    {
        public StorageSpeakingTurnReservationsEmptiedEventDTO()
        {
            EventType = StorageEventType.SpeakingTurnReservationsEmptied;
        }
    }
}
