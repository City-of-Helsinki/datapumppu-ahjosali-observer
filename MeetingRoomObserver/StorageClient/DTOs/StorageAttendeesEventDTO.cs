namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageAttendeesEventDTO : StorageEventDTO
    {
        public StorageAttendeesEventDTO()
        {
            EventType = StorageEventType.Attendees;
        }
        public List<StorageMeetingSeatDTO> MeetingSeats { get; set; } = new List<StorageMeetingSeatDTO>();
    }
}
