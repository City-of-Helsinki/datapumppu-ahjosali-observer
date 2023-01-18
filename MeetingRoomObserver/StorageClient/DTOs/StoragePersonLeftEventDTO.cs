namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StoragePersonLeftEventDTO: StorageEventDTO
    {
        public StoragePersonLeftEventDTO()
        {
            EventType = StorageEventType.PersonLeft;
        }

        public string? Person { get; set; }

        public string? SeatID { get; set; }

        public string? AdditionalInfoFI { get; set; }

        public string? AdditionalInfoSV { get; set; }
    }
}
