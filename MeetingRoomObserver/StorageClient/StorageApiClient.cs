namespace MeetingRoomObserver.StorageClient
{
    /// <summary>
    /// Queries the external storage REST API to resolve meeting identifiers.
    /// </summary>
    public interface IStorageApiClient
    {
        /// <summary>
        /// Retrieves the storage meeting identifier for a given year and sequence number.
        /// </summary>
        /// <param name="year">The meeting year.</param>
        /// <param name="sequenceNumber">The meeting sequence number within that year.</param>
        /// <returns>The meeting identifier string, or <see cref="string.Empty"/> if not found.</returns>
        Task<string> GetMeetingId(string year, string sequenceNumber);

    }

    /// <summary>
    /// Queries the external storage REST API at <c>/api/meetinginfo/meetingId/{year}/{sequenceNumber}</c>
    /// to resolve Ahjo meeting references into storage meeting identifiers.
    /// </summary>
    public class StorageApiClient : IStorageApiClient
    {
        private readonly IStorageConnection _storageConnection;
        private readonly ILogger<StorageApiClient> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageApiClient"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="storageConnection">The storage connection factory.</param>
        public StorageApiClient(ILogger<StorageApiClient> logger,
            IStorageConnection storageConnection)
        {
            _logger = logger;
            _storageConnection = storageConnection;
        }

        /// <inheritdoc />
        public async Task<string> GetMeetingId(string year, string sequenceNumber)
        {
            _logger.LogInformation("GetMeetingId {0} {1}", year, sequenceNumber);
            using var connection = _storageConnection.CreateConnection();
            var response = await connection.GetAsync($"api/meetinginfo/meetingId/{year}/{sequenceNumber}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Id not found {0} {1}", year, sequenceNumber);
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync() ?? string.Empty;
        }
    }
}
