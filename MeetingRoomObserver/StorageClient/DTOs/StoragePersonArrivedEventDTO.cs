namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StoragePersonArrivedEventDTO : StorageEventDTO
    {
        public StoragePersonArrivedEventDTO()
        {
            EventType = StorageEventType.PersonArrived;
        }

        public string? Person { get; set; }

        public string? SeatID { get; set; }

        public string? AdditionalInfoFI { get; set; }

        public string? AdditionalInfoSV { get; set; }
    }
}
