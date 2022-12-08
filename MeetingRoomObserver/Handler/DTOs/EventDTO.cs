using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
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
