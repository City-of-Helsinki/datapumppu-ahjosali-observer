using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    public class PropositionRoomDTO
    {
        [JsonProperty("hlo")]
        public string PersonFI { get; set; } = string.Empty;

        [JsonProperty("hlo_sv")]
        public string PersonSV { get; set; } = string.Empty;

        [JsonProperty("tyyppi")]
        public string PropositionType { get; set; } = string.Empty;

        [JsonProperty("tyyppi-teksti")]
        public string PropositionTypeTextFI { get; set; } = string.Empty;

        [JsonProperty("tyyppi-teksti_sv")]
        public string PropositionTypeTextSV { get; set; } = string.Empty;

        [JsonProperty("ehdotusteksti")]
        public string TextFI { get; set; } = string.Empty;

        [JsonProperty("ehdotusteksti_sv")]
        public string TextSV { get; set; } = string.Empty;
    }
}
