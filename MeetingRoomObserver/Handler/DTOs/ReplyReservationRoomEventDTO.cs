using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    /// <summary>
    /// Represents a reply reservation (vastauspuheenvuorovaraus) event from the Ahjo system.
    /// </summary>
    public class ReplyReservationRoomEventDTO : EventDTO
    {
        [JsonProperty("henkilo")]
        public string PersonFI { get; set; } = string.Empty;

        [JsonProperty("henkilo_sv")]
        public string PersonSV { get; set; } = string.Empty;

        [JsonProperty("sijanumero")]
        public int Ordinal { get; set; }

        [JsonProperty("paikka")]
        public string Seat { get; set; } = string.Empty;
    }
}
