namespace MeetingRoomObserver.StorageClient
{
    /// <summary>
    /// Defines the normalized speech types used in the storage layer.
    /// Integer values are persisted externally and must not be changed.
    /// </summary>
    public enum StorageSpeechType
    {
        /// <summary>A reply speech ("V" in the Ahjo system).</summary>
        Reply = 0, //do not change int values!
        /// <summary>A regular statement speech ("P" in the Ahjo system).</summary>
        Statement = 1,
    }
}
