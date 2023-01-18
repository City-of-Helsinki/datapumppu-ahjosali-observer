using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    public class VoteRoomDTO
    {
        [JsonProperty("hlo")]
        public string? PersonFI { get; set; }

        [JsonProperty("hlo_sv")]
        public string? PersonSV { get; set; }

        [JsonProperty("aani")]
        public string? VoteType { get; set; }
    }
}
