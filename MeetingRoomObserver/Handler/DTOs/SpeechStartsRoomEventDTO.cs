using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    /// <summary>
    /// Represents a "speech starts" event from the Ahjo system, containing the speaker, timing, direction, seat, and speech type.
    /// </summary>
    public class SpeechStartsRoomEventDTO : EventDTO
    {
        [JsonProperty("henkilo")]
        public string PersonFI { get; set; } = string.Empty;

        [JsonProperty("henkilo_sv")]
        public string PersonSV { get; set; } = string.Empty;

        [JsonProperty("puheaika")]
        public int SpeechTime { get; set; }

        [JsonProperty("puhekello")]
        public int SpeechTimer { get; set; }

        [JsonProperty("alkamisaika")]
        public DateTime StartTime { get; set; }

        [JsonProperty("suunta")]
        public string Direction { get; set; } = string.Empty;

        [JsonProperty("paikka")]
        public string Seat { get; set; } = string.Empty;

        [JsonProperty("pvtyyppi")]
        public string SpeechType { get; set; } = string.Empty;
    }
}
