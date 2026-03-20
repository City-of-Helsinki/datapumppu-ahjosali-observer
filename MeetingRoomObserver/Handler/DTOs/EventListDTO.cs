using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    /// <summary>
    /// Represents the top-level event list container from the Ahjo JSON payload, containing the meeting ID and array of events.
    /// </summary>
    public class EventListDTO
    {
        [JsonProperty("kokous")]
        public string? MeetingID { get; set; }

        [JsonProperty("tapahtumat")]
        public EventDTO[]? Events { get; set; }

    }
}
