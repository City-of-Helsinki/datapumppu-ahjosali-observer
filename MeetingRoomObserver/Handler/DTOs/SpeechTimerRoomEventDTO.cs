using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    /// <summary>
    /// Represents a speech timer update event from the Ahjo system, containing the speaker, seat, remaining time, and countdown direction.
    /// </summary>
    public class SpeechTimerRoomEventDTO : EventDTO
    {
        [JsonProperty("henkilo")]
        public string PersonFI { get; set; } = string.Empty;

        [JsonProperty("henkilo_sv")]
        public string PersonSV { get; set; } = string.Empty;

        [JsonProperty("paikka")]
        public string Seat { get; set; } = string.Empty;

        [JsonProperty("puheaika")]
        public int SpeechTime { get; set; }

        [JsonProperty("puhekello")]
        public int SpeechTimer { get; set; }

        [JsonProperty("suunta")]
        public string Direction { get; set; } = string.Empty;
    }
}
