using Confluent.Kafka;
using MeetingRoomObserver.Events.Providers;

namespace MeetingRoomObserver.Events
{
    public class AhjoSaliEventObserver : BackgroundService
    {
        private readonly ILogger<AhjoSaliEventObserver> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IKafkaClientFactory _clientFactory;
        private IMeetingMessageHandler _eventHandler;

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

                    var cr = consumer.Consume(5000);
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
                    recreateConstumer = true;
                    _logger.LogWarning("Consumer Operation Canceled.");
                    break;
                }
                catch (ConsumeException e)
                {
                    recreateConstumer = true;
                    _logger.LogError("Consumer Error: " + e.Message);
                }
                catch (Exception e)
                {
                    recreateConstumer = true;
                    _logger.LogError("Consumer Unexpected Error: " + e.Message);
                }
            }
        }

    }
}
