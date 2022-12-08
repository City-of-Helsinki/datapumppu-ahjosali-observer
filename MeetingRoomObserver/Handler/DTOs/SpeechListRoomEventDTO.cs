using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    public class SpeechListRoomEventDTO : EventDTO
    {
        [JsonProperty("pidetytpuheet")]
        public SpeechRoomDTO[] Speeches { get; set; } = new SpeechRoomDTO[0];
    }
}
