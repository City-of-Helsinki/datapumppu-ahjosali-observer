using Confluent.Kafka;

namespace MeetingRoomObserver.Events.Providers
{
    /// <summary>
    /// Factory for creating Kafka producer and consumer clients.
    /// </summary>
    public interface IKafkaClientFactory
    {
        /// <summary>
        /// Creates a new Kafka producer for publishing string messages.
        /// </summary>
        /// <returns>A configured <see cref="IProducer{TKey,TValue}"/> instance.</returns>
        public IProducer<Null, string> CreateProducer();

        /// <summary>
        /// Creates a new Kafka consumer for reading string messages.
        /// </summary>
        /// <returns>A configured <see cref="IConsumer{TKey,TValue}"/> instance.</returns>
        public IConsumer<Null, string> CreateConsumer();
    }

    /// <summary>
    /// Creates Kafka producer and consumer clients using configuration values.
    /// In development, uses plain TCP connections. In production, configures
    /// SASL-SCRAM-SHA-512 authentication with PEM SSL certificates.
    /// </summary>
    public class KafkaClientFactory : IKafkaClientFactory
    {
        private readonly IConfiguration _configuration;
        private IHostEnvironment _hostEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaClientFactory"/> class.
        /// </summary>
        /// <param name="hostEnvironment">The hosting environment, used to determine SSL/SASL configuration.</param>
        /// <param name="configuration">The application configuration containing Kafka connection settings.</param>
        public KafkaClientFactory(IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        /// <inheritdoc />
        public IProducer<Null, string> CreateProducer()
        {
            var config = CreateProducerConfiguration();

            return new ProducerBuilder<Null, string>(config).Build();
        }

        /// <inheritdoc />
        public IConsumer<Null, string> CreateConsumer()
        {
            var config = CreateConsumerConfiguration();

            return new ConsumerBuilder<Null, string>(config).Build();
        }

        private ConsumerConfig CreateConsumerConfiguration()
        {
            if (_hostEnvironment.IsDevelopment())
            {
                return new ConsumerConfig
                {
                    BootstrapServers = _configuration["KAFKA_BOOTSTRAP_SERVER"],
                    GroupId = _configuration["KAFKA_GROUP_ID"],
                };
            }

            var cert = ParseCert(_configuration["SSL_CERT_PEM"]);

            return new ConsumerConfig
            {
                BootstrapServers = _configuration["KAFKA_BOOTSTRAP_SERVER"],
                GroupId = _configuration["KAFKA_GROUP_ID"],
                SaslMechanism = SaslMechanism.ScramSha512,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = _configuration["KAFKA_USER_USERNAME"],
                SaslPassword = _configuration["KAFKA_USER_PASSWORD"],
                SslCaPem = cert,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
        }

        private ProducerConfig CreateProducerConfiguration()
        {
            if (_hostEnvironment.IsDevelopment())
            {
                return new ProducerConfig
                {
                    BootstrapServers = _configuration["KAFKA_BOOTSTRAP_SERVER"],
                };
            }

            var cert = ParseCert(_configuration["SSL_CERT_PEM"]);

            return new ProducerConfig
            {
                BootstrapServers = _configuration["KAFKA_BOOTSTRAP_SERVER"],
                SaslMechanism = SaslMechanism.ScramSha512,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = _configuration["KAFKA_USER_USERNAME"],
                SaslPassword = _configuration["KAFKA_USER_PASSWORD"],
                SslCaPem = cert
            };
        }

        private string ParseCert(string cert)
        {
            // To prevent pipeline errors the keyvault ca.crt is in quotes and without the begin/end tags. 
            cert = cert.Replace("\"", "");

            var certBegin = "-----BEGIN CERTIFICATE-----\n";
            var certEnd = "\n-----END CERTIFICATE-----";

            return certBegin + cert + certEnd;
        }

    }
}
