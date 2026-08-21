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
        private readonly IConfiguration _configuration;
        private readonly IKafkaClientFactory _clientFactory;
        private readonly IMeetingMessageHandler _eventHandler;

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
            IConfiguration configuration,
            IKafkaClientFactory clientFactory,
            IMeetingMessageHandler eventHandler
        )
        {
            _logger = logger;
            _configuration = configuration;
            _clientFactory = clientFactory;
            _eventHandler = eventHandler;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("AhjoSaliEventObserver is starting.");
            await MessageHandler(stoppingToken);
        }

        private async Task MessageHandler(CancellationToken stoppingToken)
        {
            var topic = _configuration["KAFKA_CONSUMER_TOPIC"];
            if (string.IsNullOrEmpty(topic))
            {
                _logger.LogError("KAFKA_CONSUMER_TOPIC not configured.");
                return;
            }

            var consumer = _clientFactory.CreateConsumer();
            consumer.Subscribe(topic);
            bool recreateConsumer = false;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (recreateConsumer)
                    {
                        _logger.LogWarning("Recreating consumer");
                        consumer = _clientFactory.CreateConsumer();
                        consumer.Subscribe(topic);
                        recreateConsumer = false;
                    }

                    var cr = consumer.Consume(stoppingToken);
                    if (cr != null && !string.IsNullOrEmpty(cr.Message.Value))
                    {
                        _logger.LogInformation("AhjoSali event received");
                        await _eventHandler.HandleMessage(cr.Message.Value);
                        consumer.Commit(cr);
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning("Consumer Operation Canceled.");
                    break;
                }
                catch (ConsumeException e)
                {
                    _logger.LogError("Consumer Error: {Message}", e.Message);
                    consumer.Close();
                    recreateConsumer = true;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Consumer Unexpected Error: {Message}", e.Message);
                    consumer.Close();
                    recreateConsumer = true;
                    await Task.Delay(1000, stoppingToken); // Avoid tight loop on repeated failures
                }
            }

            consumer.Close();
        }
    }
}
