namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Storage event DTO for a case/agenda item, containing proposition, case text, item text, and identifier in Finnish and Swedish.
    /// </summary>
    public class StorageCaseEventDTO : StorageEventDTO
    {
        public StorageCaseEventDTO()
        {
            EventType = StorageEventType.Case;
        }

        public string? PropositionFI { get; set; } = string.Empty;

        public string? PropositionSV { get; set; } = string.Empty;

        public string? CaseTextFI { get; set; } = string.Empty;

        public string? CaseTextSV { get; set; } = string.Empty;

        public string? ItemTextFI { get; set; } = string.Empty;

        public string? ItemTextSV { get; set; } = string.Empty;

        public string? Identifier { get; set; } = string.Empty;
    }
}
