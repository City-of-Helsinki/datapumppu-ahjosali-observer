namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO indicating that all statement reservations have been cleared.
    /// </summary>
    public class StorageStatementReservationsClearedEventDTO: StorageEventDTO
    {
        public StorageStatementReservationsClearedEventDTO()
        {
            EventType = StorageEventType.StatementReservationsCleared;
        }
    }
}
