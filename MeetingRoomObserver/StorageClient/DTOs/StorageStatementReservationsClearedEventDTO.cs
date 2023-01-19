namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageStatementReservationsClearedEventDTO: StorageEventDTO
    {
        public StorageStatementReservationsClearedEventDTO()
        {
            EventType = StorageEventType.StatementReservationsCleared;
        }
    }
}
