namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageRollCallEndedEventDTO: StorageEventDTO
    {
        public StorageRollCallEndedEventDTO()
        {
            EventType = StorageEventType.RollCallEnded;
        }

        public int? Present { get; set; }

        public int? Absent { get; set; }
    }
}
