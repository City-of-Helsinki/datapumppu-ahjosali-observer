namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Represents a proposition for storage, with text in Finnish and Swedish, person, type, and additional info.
    /// </summary>
    public class StoragePropositionDTO
    {
        public string? TextFI { get; set; } = string.Empty;

        public string? TextSV { get; set; } = string.Empty;

        public string? Person { get; set; } = string.Empty;

        public string? Type { get; set; } = string.Empty;

        public string? TypeTextFI { get; set; } = string.Empty;

        public string? TypeTextSV { get; set; } = string.Empty;

        public string? AdditionalInfoFI { get; set; } = string.Empty;

        public string? AdditionalInfoSV { get; set; } = string.Empty;
    }
}
