namespace MeetingRoomObserver.StorageClient
{
    /// <summary>
    /// Creates HTTP connections to the external storage REST API.
    /// </summary>
    public interface IStorageConnection
    {
        /// <summary>
        /// Creates a new <see cref="HttpClient"/> with the base address set to the configured storage URL.
        /// </summary>
        /// <returns>A configured <see cref="HttpClient"/> instance.</returns>
        HttpClient CreateConnection();
    }

    /// <summary>
    /// Creates <see cref="HttpClient"/> instances configured with the <c>STORAGE_URL</c> base address
    /// for communicating with the external storage REST API.
    /// </summary>
    public class StorageConnection : IStorageConnection
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageConnection"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration containing the <c>STORAGE_URL</c> key.</param>
        public StorageConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc />
        public HttpClient CreateConnection()
        {
            var connection = new HttpClient();
            connection.BaseAddress = new Uri(_configuration["STORAGE_URL"]);
            return connection;
        }
    }
}
