namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageSpeakingTurnEndedEventDTO: StorageEventDTO
    {
        public StorageSpeakingTurnEndedEventDTO()
        {
            EventType = StorageEventType.SpeakingTurnEnded;
        }
    }
}
