namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO for pause information, containing an informational text.
    /// </summary>
    public class StoragePauseInfoEventDTO: StorageEventDTO
    {
        public string Info { get; set; }

        public StoragePauseInfoEventDTO()
        {
            EventType = StorageEventType.PauseInfo;
        }
    }
}
