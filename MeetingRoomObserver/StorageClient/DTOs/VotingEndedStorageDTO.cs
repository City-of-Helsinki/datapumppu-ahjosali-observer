namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class VotingEndedStorageDTO : StorageEventDTO
    {
        public VotingEndedStorageDTO()
        {
            EventType = StorageClient.StorageEventType.VotingStarted;
        }

        public int VotingNumber { get; set; }

        public int VotingType { get; set; }

        public string? VotingTypeTextFI { get; set; }

        public string? VotingTypeTextSV { get; set; }

        public string ForTextFI { get; set; } = string.Empty;

        public string ForTextSV { get; set; } = string.Empty;

        public string ForTitleFI { get; set; } = string.Empty;

        public string ForTitleSV { get; set; } = string.Empty;

        public string AgainstTextFI { get; set; } = string.Empty;

        public string AgainstTextSV { get; set; } = string.Empty;

        public string AgainstTitleFI { get; set; } = string.Empty;

        public string AgainstTitleSV { get; set; } = string.Empty;

        public int VotesFor { get; set; }

        public int VotesAgainst { get; set; }

        public int VotesEmpty { get; set; }

        public int VotesAbsent { get; set; }

        public List<VoteStorageDTO>? Votes { get; set; }


    }
}
