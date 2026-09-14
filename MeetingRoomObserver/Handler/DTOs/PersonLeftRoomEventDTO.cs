using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    /// <summary>
    /// Represents a "person left" event from the Ahjo system, containing the person name in Finnish and Swedish and their seat number.
    /// </summary>
    public class PersonLeftRoomEventDTO : EventDTO
    {
        [JsonProperty("henkilo")]
        public string PersonFI { get; set; } = string.Empty;

        [JsonProperty("henkilo_sv")]
        public string PersonSV { get; set; } = string.Empty;

        [JsonProperty("paikka")]
        public string Seat { get; set; } = string.Empty;
    }
}
