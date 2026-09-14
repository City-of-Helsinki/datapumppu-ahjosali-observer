namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO indicating that a roll call has ended, containing the count of present and absent members.
    /// </summary>
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
