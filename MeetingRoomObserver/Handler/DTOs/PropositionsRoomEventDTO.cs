using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    /// <summary>
    /// Represents a propositions event from the Ahjo system, containing a list of propositions made during the meeting.
    /// </summary>
    public class PropositionsRoomEventDTO : EventDTO
    {
        [JsonProperty("ehdotukset")]
        public PropositionRoomDTO[] Propositions { get; set; }
    }
}
