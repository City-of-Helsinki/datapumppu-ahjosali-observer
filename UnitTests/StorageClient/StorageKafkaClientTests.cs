using MeetingRoomObserver.StorageClient;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using MeetingRoomObserver.Events.Providers;
using Microsoft.Extensions.Configuration;
using MeetingRoomObserver.StorageClient.DTOs;
using Confluent.Kafka;

namespace StorageClient.Tests
{
    public class StorageKafkaClientTests
    {
        [Fact]
        public async void ShouldCreateNewProducerWhenCalledFirstTime()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<StorageKafkaClient>>();
            var mockClientFactory = new Mock<IKafkaClientFactory>();
            var client = new StorageKafkaClient(mockConfiguration.Object, mockLogger.Object, mockClientFactory.Object);

            await client.SendEvent(new StorageEventDTO());
            mockClientFactory.Verify(x => x.CreateProducer(), Times.Once);
        }

        [Fact]
        public async void ShouldNotCreateNewProducerWhenCalledSecondTime()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<StorageKafkaClient>>();
            var mockClientFactory = new Mock<IKafkaClientFactory>();
            var client = new StorageKafkaClient(mockConfiguration.Object, mockLogger.Object, mockClientFactory.Object);

            mockClientFactory.Setup(x => x.CreateProducer()).Returns(new Mock<IProducer<Null, string>>().Object);

            await client.SendEvent(new StorageEventDTO());
            await client.SendEvent(new StorageEventDTO());
            mockClientFactory.Verify(x => x.CreateProducer(), Times.Once);
        }

        [Fact]
        public async void ShouldCallProduceAsync()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<StorageKafkaClient>>();
            var mockClientFactory = new Mock<IKafkaClientFactory>();
            var client = new StorageKafkaClient(mockConfiguration.Object, mockLogger.Object, mockClientFactory.Object);

            var mockProducer = new Mock<IProducer<Null, string>>();
            mockClientFactory.Setup(x => x.CreateProducer()).Returns(mockProducer.Object);

            await client.SendEvent(new StorageEventDTO());

            mockClientFactory.Verify(x => x.CreateProducer(), Times.Once);
            mockProducer.Verify(x => x.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>(), new CancellationToken()), Times.Once);
        }
    }
}