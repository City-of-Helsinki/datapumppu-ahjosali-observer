namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StoragePersonArrivedEventDTO : StorageEventDTO
    {
        public StoragePersonArrivedEventDTO()
        {
            EventType = StorageEventType.PersonArrived;
        }

        public string? PersonFI { get; set; }

        public string? PersonSV { get; set; }

        public string? SeatID { get; set; }
    }
}
