namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StoragePauseInfoEventDTO: StorageEventDTO
    {
        public string Info { get; set; }

        public StoragePauseInfoEventDTO()
        {
            EventType = StorageEventType.PauseInfo;
        }
    }
}
