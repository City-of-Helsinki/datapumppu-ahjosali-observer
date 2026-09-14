namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO for meeting attendees, containing a list of seat assignments.
    /// </summary>
    public class StorageAttendeesEventDTO : StorageEventDTO
    {
        public StorageAttendeesEventDTO()
        {
            EventType = StorageEventType.Attendees;
        }
        public List<StorageMeetingSeatDTO> MeetingSeats { get; set; } = new List<StorageMeetingSeatDTO>();
    }
}
