namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageSpeakingTurnsEventDTO: StorageEventDTO
    {
        public StorageSpeakingTurnsEventDTO()
        {
            EventType = StorageEventType.SpeakingTurns;
        }

        public List<StorageSpeakingTurnDTO> SpeakingTurns { get; set; }
    }
}
