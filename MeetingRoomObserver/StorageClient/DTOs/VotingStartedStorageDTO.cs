namespace MeetingRoomObserver.StorageClient.DTOs
{
    public class VotingStartedStorageDTO : StorageEventDTO
    {
        public VotingStartedStorageDTO()
        {
            EventType = StorageClient.StorageEventType.VotingStarted;
        }

        public int VotingType { get; set; }

        public string? VotingTypeText { get; set; }

        public string? ForText { get; set; }

        public string? ForTitle { get; set; }

        public string? AgainstText { get; set; }

        public string? AgainstTitle { get; set; }

    }
}
