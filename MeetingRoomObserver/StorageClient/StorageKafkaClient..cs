using Confluent.Kafka;
using MeetingRoomObserver.StorageClient.DTOs;
using MeetingRoomObserver.Events.Providers;
using Newtonsoft.Json;

namespace MeetingRoomObserver.StorageClient
{
    /// <summary>
    /// Publishes storage event DTOs to an outbound Kafka topic.
    /// </summary>
    public interface IStorageKafkaClient
    {
        /// <summary>
        /// Serializes and sends the given storage event to the configured Kafka producer topic.
        /// </summary>
        /// <param name="storageEventDTO">The storage event to publish.</param>
        Task SendEvent(StorageEventDTO storageEventDTO);
    }

    /// <summary>
    /// Produces serialized <see cref="StorageEventDTO"/> messages to the Kafka topic
    /// specified by the <c>KAFKA_PRODUCER_TOPIC</c> configuration key.
    /// Lazily creates and reuses a single <see cref="IProducer{TKey,TValue}"/> instance.
    /// </summary>
    public class StorageKafkaClient : IStorageKafkaClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<StorageKafkaClient> _logger;
        private IKafkaClientFactory _clientFactory;
        private IProducer<Null, string>? _producer = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageKafkaClient"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="logger">The logger instance.</param>
        /// <param name="clientFactory">The factory used to create Kafka producers.</param>
        public StorageKafkaClient(
            IConfiguration configuration,
            ILogger<StorageKafkaClient> logger,
            IKafkaClientFactory clientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _clientFactory = clientFactory;
        }

        /// <inheritdoc />
        public async Task SendEvent(StorageEventDTO storageEventDTO)
        {
            _logger.LogInformation("Sending event to Storage");
            var topic = _configuration["KAFKA_PRODUCER_TOPIC"];

            if (_producer == null)
            {
                _producer = _clientFactory.CreateProducer();
            }

            var jsonBody = JsonConvert.SerializeObject(storageEventDTO);

            try
            {
                if (_producer != null)
                {
                    await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonBody });
                }                
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Producer failed");
                _producer = null;
            }
        }

    }
}
