namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class AttendeesEventStorageDTO : StorageEventDTO
    {
        public List<MeetingSeatStorageDTO> MeetingSeats { get; set; } = new List<MeetingSeatStorageDTO>();
    }
}
