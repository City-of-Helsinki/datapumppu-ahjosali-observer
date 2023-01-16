using Confluent.Kafka;
using MeetingRoomObserver.StorageClient.DTOs;
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

        public StorageKafkaClient(IConfiguration configuration, IHostEnvironment hostEnvironment, ILogger<StorageKafkaClient> logger)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            _logger = logger; 
        }

        public Task SendEvent(StorageEventDTO storageEventDTO)
        {
            _logger.LogInformation("Sending event to Storage");
            var topic = _configuration["KAFKA_TOPIC"];

            var producer = CreateKafkaProducer();
            var jsonBody = JsonConvert.SerializeObject(storageEventDTO);

            return producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonBody });
        }

        private ProducerConfig CreateKafkaConfiguration()
        {
            if (_hostEnvironment.IsDevelopment())
            {
                return new ProducerConfig
                {
                    BootstrapServers = _configuration["KAFKA_BOOTSTRAP_SERVER"],
                };
            }
            else
            {
                return new ProducerConfig
                {
                    BootstrapServers = _configuration["KAFKA_BOOTSTRAP_SERVER"],
                    SaslMechanism = SaslMechanism.ScramSha512,
                    SecurityProtocol = SecurityProtocol.SaslSsl,
                    SaslUsername = _configuration["KAFKA_USER_USERNAME"],
                    SaslPassword = _configuration["KAFKA_USER_PASSWORD"],
                    SslCertificatePem = _configuration["SSL_CERT_PEM"]
                };
            }
        }

        private IProducer<Null, string> CreateKafkaProducer()
        {
            var config = CreateKafkaConfiguration();

            return new ProducerBuilder<Null, string>(config).Build();
        }

    }
}
