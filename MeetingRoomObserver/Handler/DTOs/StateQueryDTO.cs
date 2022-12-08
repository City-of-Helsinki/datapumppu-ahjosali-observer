using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
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
    }
}
