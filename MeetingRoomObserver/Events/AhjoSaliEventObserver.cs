using Confluent.Kafka;
using MeetingRoomObserver.Events.Providers;

namespace MeetingRoomObserver.Events
{
    /// <summary>
    /// Background service that continuously consumes meeting room events from the
    /// Kafka topic configured by <c>KAFKA_CONSUMER_TOPIC</c> and forwards them to
    /// <see cref="IMeetingMessageHandler"/> for processing.
    /// Automatically recreates the Kafka consumer on transient failures.
    /// </summary>
    public class AhjoSaliEventObserver : BackgroundService
    {
        private readonly ILogger<AhjoSaliEventObserver> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IKafkaClientFactory _clientFactory;
        private IMeetingMessageHandler _eventHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AhjoSaliEventObserver"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="serviceProvider">The service provider for scoped dependency resolution.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="hostEnvironment">The hosting environment.</param>
        /// <param name="clientFactory">The factory used to create Kafka consumers.</param>
        /// <param name="eventHandler">The handler that processes received messages.</param>
        public AhjoSaliEventObserver(
            ILogger<AhjoSaliEventObserver> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            IHostEnvironment hostEnvironment,
            IKafkaClientFactory clientFactory,
            IMeetingMessageHandler eventHandler
        )
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            _clientFactory = clientFactory;
            _eventHandler = eventHandler;
        }

        /// <inheritdoc />
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => MessageHandler(stoppingToken));
        }

        private async Task MessageHandler(CancellationToken stoppingToken)
        {
            var topic = _configuration["KAFKA_CONSUMER_TOPIC"];
            var consumer = _clientFactory.CreateConsumer();
            consumer.Subscribe(topic);
            bool recreateConstumer = false;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (recreateConstumer)
                    {
                        _logger.LogWarning("recreating consumer");
                        consumer = _clientFactory.CreateConsumer();
                        consumer.Subscribe(topic);
                        recreateConstumer = false;
                    }

                    var cr = consumer.Consume(stoppingToken);
                    if (cr != null && !string.IsNullOrEmpty(cr.Message.Value))
                    {
                        _logger.LogInformation("AhjoSali event received");
                        _logger.LogInformation(cr.Message.Value);

                        await _eventHandler.HandleMessage(cr.Message.Value);

                        consumer.Commit(cr);
                    }

                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                    recreateConstumer = true;
                    _logger.LogWarning("Consumer Operation Canceled.");
                    break;
                }
                catch (ConsumeException e)
                {
                    consumer.Close();
                    recreateConstumer = true;
                    _logger.LogError("Consumer Error: " + e.Message);
                }
                catch (Exception e)
                {
                    consumer.Close();
                    recreateConstumer = true;
                    _logger.LogError("Consumer Unexpected Error: " + e.Message);
                }
            }
        }

    }
}
