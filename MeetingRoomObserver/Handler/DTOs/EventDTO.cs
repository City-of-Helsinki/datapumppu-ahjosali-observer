using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    /// <summary>
    /// Base class for all Ahjo meeting room events, containing the sequence number, timestamp, and event type.
    /// </summary>
    public class EventDTO
    {
        [JsonProperty("snro")]
        public long SequenceNumber { get; set; }

        [JsonProperty("aikaleima")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("laji")]
        public string? EventType { get; set; }
    }
}
