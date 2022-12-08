using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    public class EventListDTO
    {
        [JsonProperty("kokous")]
        public string? MeetingID { get; set; }

        [JsonProperty("tapahtumat")]
        public EventDTO[]? Events { get; set; }

    }
}
