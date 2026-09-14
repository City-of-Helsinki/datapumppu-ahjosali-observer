using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    /// <summary>
    /// Represents an individual vote cast by a member, with person name in Finnish and Swedish and the vote type.
    /// </summary>
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
