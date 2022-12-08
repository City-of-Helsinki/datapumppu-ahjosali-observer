using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    public class SpeechEndsRoomEventDTO : EventDTO
    {
        [JsonProperty("henkilo")]
        public string PersonFI { get; set; } = string.Empty;

        [JsonProperty("henkilo_sv")]
        public string PersonSV { get; set; } = string.Empty;

        [JsonProperty("paikka")]
        public string Seat { get; set; } = string.Empty;
    }
}
