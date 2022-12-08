using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    public class PauseInfoRoomEventDTO : EventDTO
    {
        [JsonProperty("tiedote")]
        public string Info { get; set; } = string.Empty;
    }
}
