namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO containing a list of propositions made during the meeting.
    /// </summary>
    public class StoragePropositionsEventDTO: StorageEventDTO
    {
        public StoragePropositionsEventDTO()
        {
            EventType = StorageEventType.Propositions;
        }

        public List<StoragePropositionDTO> Propositions { get; set; }
    }
}
