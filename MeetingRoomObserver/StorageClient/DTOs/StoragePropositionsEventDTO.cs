namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StoragePropositionsEventDTO: StorageEventDTO
    {
        public StoragePropositionsEventDTO()
        {
            EventType = StorageEventType.Propositions;
        }

        public List<StoragePropositionDTO> Propositions { get; set; }
    }
}
