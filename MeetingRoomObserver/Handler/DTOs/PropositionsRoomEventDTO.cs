using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    public class PropositionsRoomEventDTO : EventDTO
    {
        [JsonProperty("ehdotukset")]
        public PropositionRoomDTO[] Propositions { get; set; }
    }
}
