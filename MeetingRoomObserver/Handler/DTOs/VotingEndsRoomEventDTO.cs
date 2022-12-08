using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    public class VotingEndsRoomEventDTO : EventDTO
    {
        [JsonProperty("jaa")]
        public int AyeCount { get; set; }

        [JsonProperty("ei")]
        public int NayCount { get; set; }

        [JsonProperty("tyhjia")]
        public int EmptyCount { get; set; }

        [JsonProperty("poissa")]
        public int AbsentCount { get; set; }

        [JsonProperty("numero")]
        public int Number { get; set; }

        [JsonProperty("tyyppi")]
        public string? VotingType { get; set; }

        [JsonProperty("tyyppi-teksti")]
        public string? VotingTypeTextFI { get; set; }

        [JsonProperty("tyyppi-teksti_sv")]
        public string? VotingTypeTextSV { get; set; }

        [JsonProperty("jaa-teksti")]
        public string? AyeTextFI { get; set; }

        [JsonProperty("jaa-teksti_sv")]
        public string? AyeTextSV { get; set; }

        [JsonProperty("jaa-ots")]
        public string? AyeTitleFI { get; set; }

        [JsonProperty("jaa-ots_sv")]
        public string? AyeTitleSV { get; set; }

        [JsonProperty("ei-teksti")]
        public string? NayTextFI { get; set; }

        [JsonProperty("ei-teksti_sv")]
        public string? NayTextSV { get; set; }

        [JsonProperty("ei-ots")]
        public string? NayTitleFI { get; set; }

        [JsonProperty("ei-ots_sv")]
        public string? NayTitleSV { get; set; }

        [JsonProperty("aanet")]
        public VoteRoomDTO[]? Votes { get; set; }
    }
}
