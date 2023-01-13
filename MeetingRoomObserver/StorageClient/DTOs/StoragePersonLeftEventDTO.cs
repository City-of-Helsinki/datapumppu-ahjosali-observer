namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StoragePersonLeftEventDTO: StorageEventDTO
    {
        public StoragePersonLeftEventDTO()
        {
            EventType = StorageEventType.PersonLeft;
        }

        public string? PersonFI { get; set; }

        public string? PersonSV { get; set; }

        public string? SeatID { get; set; }
    }
}
