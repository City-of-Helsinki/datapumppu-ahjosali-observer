using MeetingRoomObserver.StorageClient.DTOs;

namespace MeetingRoomObserver.StorageClient
{
    /// <summary>
    /// Routes storage events for persistence.
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Validates and forwards a storage event for persistence via Kafka.
        /// Events without a valid <see cref="StorageEventDTO.MeetingID"/> are discarded.
        /// </summary>
        /// <param name="storageEventDTO">The storage event to persist.</param>
        Task Add(StorageEventDTO storageEventDTO);
    }

    /// <summary>
    /// Validates that events have a non-empty meeting ID and forwards them
    /// to the <see cref="IStorageKafkaClient"/> for publication on Kafka.
    /// </summary>
    public class Storage : IStorage
    {
        private readonly ILogger<Storage> _logger;
        private readonly IStorageKafkaClient _kafkaClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="Storage"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="kafkaClient">The Kafka client used to publish events.</param>
        public Storage(ILogger<Storage> logger, IStorageKafkaClient kafkaClient)
        {
            _logger = logger;
            _kafkaClient = kafkaClient;
        }

        /// <inheritdoc />
        public Task Add(StorageEventDTO storageEventDTO)
        {
            if (string.IsNullOrEmpty(storageEventDTO.MeetingID))
            {
                _logger.LogError("unknown meeting id, ignoring event");
                return Task.CompletedTask;
            }
            return _kafkaClient.SendEvent(storageEventDTO);
        }
    }
}
