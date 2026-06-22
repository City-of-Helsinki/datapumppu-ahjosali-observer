using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    /// <summary>
    /// Represents the current state of the meeting, including titles in Finnish and Swedish, case and item numbers, and the last sequence number.
    /// </summary>
    public class StateQueryDTO
    {
        [JsonProperty("kokousotsikko")]
        public string? MeetingTitleFI { get; set; }

        [JsonProperty("kokousotsikko_sv")]
        public string? MeetingTitleSV { get; set; }

        [JsonProperty("asianumero")]
        public string CaseNumber { get; set; } = string.Empty;

        [JsonProperty("kohtanumero")]
        public string ItemNumber { get; set; } = string.Empty;

        [JsonProperty("snro")]
        public long? SequenceNumber { get; set; }
    }
}
