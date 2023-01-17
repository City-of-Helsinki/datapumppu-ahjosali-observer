namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class StorageVotingStartedEventDTO : StorageEventDTO
    {
        public StorageVotingStartedEventDTO()
        {
            EventType = StorageEventType.VotingStarted;
        }

        public int VotingNumber { get; set; }

        public int VotingType { get; set; }

        public string? VotingTypeTextFI { get; set; }

        public string? VotingTypeTextSV { get; set; }

        public string ForTextFI { get; set; }

        public string ForTextSV { get; set; }

        public string ForTitleFI { get; set; }

        public string ForTitleSV { get; set; }

        public string AgainstTextFI { get; set; }

        public string AgainstTextSV { get; set; }

        public string AgainstTitleFI { get; set; }

        public string AgainstTitleSV { get; set; }
    }
}
