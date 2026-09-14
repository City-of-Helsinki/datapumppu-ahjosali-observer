using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    /// <summary>
    /// Represents a pause information event from the Ahjo system, containing a bulletin text.
    /// </summary>
    public class PauseInfoRoomEventDTO : EventDTO
    {
        [JsonProperty("tiedote")]
        public string Info { get; set; } = string.Empty;
    }
}
