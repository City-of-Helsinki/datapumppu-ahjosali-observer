namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO containing a list of statements (delivered speeches) during the meeting.
    /// </summary>
    public class StorageStatementsEventDTO: StorageEventDTO
    {
        public StorageStatementsEventDTO()
        {
            EventType = StorageEventType.Statements;
        }

        public List<StorageStatementDTO> Statements { get; set; }
    }
}
