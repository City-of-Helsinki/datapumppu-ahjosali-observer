using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    public class RollCallEndsRoomEventDTO : EventDTO
    {
        [JsonProperty("lasna")]
        public int Present { get; set; }

        [JsonProperty("poissa")]
        public int Absent { get; set; }

    }
}
