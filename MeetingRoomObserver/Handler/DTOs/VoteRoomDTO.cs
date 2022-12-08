using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    public class VoteRoomDTO
    {
        [JsonProperty("hlo")]
        public string? NameFI { get; set; }

        [JsonProperty("hlo_sv")]
        public string? NameSV { get; set; }

        [JsonProperty("aani")]
        public string? VoteType { get; set; }
    }
}
