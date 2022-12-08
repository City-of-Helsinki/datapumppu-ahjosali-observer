namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class CaseStorageDTO : StorageEventDTO
    {
        public CaseStorageDTO()
        {
            EventType = StorageEventType.Case;
        }

        public string PropositionFI { get; set; } = string.Empty;

        public string PropositionSV { get; set; } = string.Empty;

        public string CaseNumber { get; set; } = string.Empty;

        public string ItemNumber { get; set; } = string.Empty;
    }
}
