using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    /// <summary>
    /// Represents a "roll call ends" event from the Ahjo system, containing the count of present and absent members.
    /// </summary>
    public class RollCallEndsRoomEventDTO : EventDTO
    {
        [JsonProperty("lasna")]
        public int Present { get; set; }

        [JsonProperty("poissa")]
        public int Absent { get; set; }

    }
}
