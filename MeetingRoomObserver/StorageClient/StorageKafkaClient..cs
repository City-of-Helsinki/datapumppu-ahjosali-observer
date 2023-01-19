using Confluent.Kafka;
using MeetingRoomObserver.StorageClient.DTOs;
using MeetingRoomObserver.Events.Providers;
using Newtonsoft.Json;

namespace MeetingRoomObserver.StorageClient
{
    public interface IStorageKafkaClient
    {
        Task SendEvent(StorageEventDTO storageEventDTO);
    }

    public class StorageKafkaClient : IStorageKafkaClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<StorageKafkaClient> _logger;
        private IHostEnvironment _hostEnvironment;
        private IKafkaClientFactory _clientFactory;

        public StorageKafkaClient(
            IConfiguration configuration,
            IHostEnvironment hostEnvironment,
            ILogger<StorageKafkaClient> logger,
            IKafkaClientFactory clientFactory)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public Task SendEvent(StorageEventDTO storageEventDTO)
        {
            _logger.LogInformation("Sending event to Storage");
            var topic = _configuration["KAFKA_PRODUCER_TOPIC"];

            var producer = _clientFactory.CreateProducer();
            var jsonBody = JsonConvert.SerializeObject(storageEventDTO);

            return producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonBody });
        }

    }
}
