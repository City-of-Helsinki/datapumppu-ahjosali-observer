using Microsoft.Extensions.Logging;
using Xunit;
using Moq;
using MeetingRoomObserver.StorageClient;
using MeetingRoomObserver.StorageClient.DTOs;

namespace StorageClient.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void AddShouldCallKafkaClient()
        {
            // Arrange
            var mockKafkaClient = new Mock<IStorageKafkaClient>();
            var mockLogger = new Mock<ILogger<Storage>>();
            var storageClient = new Storage(mockLogger.Object, mockKafkaClient.Object);

            // Act
            storageClient.Add(new StorageEventDTO { MeetingID = "123" });

            // Assert
            mockKafkaClient.Verify(x => x.SendEvent(It.IsAny<StorageEventDTO>()), Times.Once);
        }

        [Fact]
        public void IfMeetingIdIsNullAddShouldNotCallKafkaClient()
        {
            // Arrange
            var mockKafkaClient = new Mock<IStorageKafkaClient>();
            var mockLogger = new Mock<ILogger<Storage>>();
            var storageClient = new Storage(mockLogger.Object, mockKafkaClient.Object);

            // Act
            storageClient.Add(new StorageEventDTO() { MeetingID = null });

            // Assert
            mockKafkaClient.Verify(x => x.SendEvent(It.IsAny<StorageEventDTO>()), Times.Never);
        }

        [Fact]
        public void IfMeetingIdIsEmptyStringAddShouldNotCallKafkaClient()
        {
            // Arrange
            var mockKafkaClient = new Mock<IStorageKafkaClient>();
            var mockLogger = new Mock<ILogger<Storage>>();
            var storageClient = new Storage(mockLogger.Object, mockKafkaClient.Object);

            // Act
            storageClient.Add(new StorageEventDTO() { MeetingID = string.Empty });

            // Assert
            mockKafkaClient.Verify(x => x.SendEvent(It.IsAny<StorageEventDTO>()), Times.Never);
        }

    }
}