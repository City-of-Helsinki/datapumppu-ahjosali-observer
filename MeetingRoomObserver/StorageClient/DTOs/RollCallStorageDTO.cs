namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class RollCallStorageDTO : StorageEventDTO
    {
        public int? Present { get; set; }

        public int? Absent { get; set; }

    }
}
