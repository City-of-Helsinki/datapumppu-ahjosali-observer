using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    /// <summary>
    /// Represents a list of delivered speeches event from the Ahjo system.
    /// </summary>
    public class SpeechListRoomEventDTO : EventDTO
    {
        [JsonProperty("pidetytpuheet")]
        public SpeechRoomDTO[] Speeches { get; set; } = new SpeechRoomDTO[0];
    }
}
