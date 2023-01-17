using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    public class SpeechRoomDTO
    {
        [JsonProperty("henkilo")]
        public string PersonFI { get; set; } = string.Empty;

        [JsonProperty("henkilo_sv")]
        public string PersonSV { get; set; } = string.Empty;

        [JsonProperty("alkamisaika")]
        public DateTime StartTime { get; set; }

        [JsonProperty("tyyppi")]
        public string SpeechType { get; set; } = string.Empty;

        [JsonProperty("puheenkesto")]
        public int Duration { get; set; }

        [JsonProperty("paattymisaika")]
        public DateTime EndTime { get; set; }
    }
}
