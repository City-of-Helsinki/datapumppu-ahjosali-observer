namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageStatementsEventDTO: StorageEventDTO
    {
        public StorageStatementsEventDTO()
        {
            EventType = StorageEventType.Statements;
        }

        public List<StorageStatementDTO> Statements { get; set; }
    }
}
