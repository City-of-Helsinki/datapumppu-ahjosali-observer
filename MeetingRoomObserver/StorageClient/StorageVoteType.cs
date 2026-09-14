namespace MeetingRoomObserver.StorageClient
{
    /// <summary>
    /// Defines the normalized vote types used in the storage layer.
    /// Maps from Finnish vote values: JAA, EI, TYHJA, POISSA.
    /// </summary>
    public enum StorageVoteType
    {
        /// <summary>A vote in favor ("JAA").</summary>
        Aye,
        /// <summary>A vote against ("EI").</summary>
        Nay,
        /// <summary>An empty/abstained vote ("TYHJA").</summary>
        Empty,
        /// <summary>The voter was absent ("POISSA").</summary>
        Absent
    }
}
