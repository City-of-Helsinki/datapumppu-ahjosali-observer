namespace MeetingRoomObserver.StorageClient
{
    public interface IStorageApiClient
    {
        Task<string> GetMeetingId(string year, string sequenceNumber);

    }

    public class StorageApiClient : IStorageApiClient
    {
        private readonly IStorageConnection _storageConnection;
        private readonly ILogger<StorageApiClient> _logger;

        public StorageApiClient(ILogger<StorageApiClient> logger,
            IStorageConnection storageConnection)
        {
            _logger = logger;
            _storageConnection = storageConnection;
        }

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
