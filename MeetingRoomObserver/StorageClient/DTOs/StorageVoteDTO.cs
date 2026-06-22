namespace MeetingRoomObserver.StorageClient.DTOs
{
    /// <summary>
    /// Represents an individual vote for storage, with person, numeric vote type, and additional info in Finnish and Swedish.
    /// </summary>
    public class StorageVoteDTO
    {
        public string? Person { get; set; }

        public int VoteType { get; set; }

        public string? AdditionalInfoFI { get; set; }

        public string? AdditionalInfoSV { get; set; }
    }
}
