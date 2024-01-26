using MeetingRoomObserver.Handler.DTOs;
using MeetingRoomObserver.Mapper;
using MeetingRoomObserver.StorageClient;
using Xunit;

public class MeetingEventTypeMapperTests
{
    [Fact]
    public void NormalShouldBeMappedTo0()
    {
        // Arrange
        var mapper = new MeetingEventTypeMapper();

        // Act
        var result = mapper.MapToMeetingEventType(EventTypeDTOConstants.RollCallEnds);

        // Assert
        Assert.Equal(StorageEventType.RollCallEnded, result);
    }

    [Fact]
    public void UnknownTypeShouldThrowException()
    {
        // Arrange
        var mapper = new MeetingEventTypeMapper();

        // Act
        var exception = Record.Exception(() => mapper.MapToMeetingEventType("UNKNOWN"));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<NotSupportedException>(exception);
    }
}