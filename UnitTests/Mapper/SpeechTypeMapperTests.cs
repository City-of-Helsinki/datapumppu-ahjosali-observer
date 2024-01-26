using MeetingRoomObserver.Mapper;
using Xunit;

public class SpeectTypeMapperTests
{
    [Fact]
    public void MaleShouldBeMappedTo0()
    {
        // Arrange
        var mapper = new SpeechTypeMapper();

        // Act
        var result = mapper.MapToSpeechType("V");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void FemaleShouldBeMappedTo1()
    {
        // Arrange
        var mapper = new SpeechTypeMapper();

        // Act
        var result = mapper.MapToSpeechType("P");

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void UnknownTypeShouldThrowException()
    {
        // Arrange
        var mapper = new SpeechTypeMapper();

        // Act
        var exception = Record.Exception(() => mapper.MapToSpeechType("UNKNOWN"));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<NotSupportedException>(exception);
    }
}