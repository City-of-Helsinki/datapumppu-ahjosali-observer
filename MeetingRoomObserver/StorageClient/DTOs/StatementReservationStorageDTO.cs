
namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StatementReservationStorageDTO : StorageEventDTO
    {
        public string Person { get; set; } = string.Empty;

        public string AdditionalInfoFI { get; set; } = string.Empty;

        public string AdditionalInfoSV { get; set; } = string.Empty;

        public int Ordinal { get; set; }

        public string SeatID { get; set; } = string.Empty;
    }
}
