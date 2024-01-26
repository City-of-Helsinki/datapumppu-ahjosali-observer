using MeetingRoomObserver.Mapper;
using Xunit;

public class VotingTypeMapperTests
{
    [Fact]
    public void NormaShouldBeMappedTo0()
    {
        // Arrange
        var mapper = new VotingTypeMapper();

        // Act
        var result = mapper.MapToVotingType("NORMAL");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void NormaaliShouldBeMappedTo0()
    {
        // Arrange
        var mapper = new VotingTypeMapper();

        // Act
        var result = mapper.MapToVotingType("NORMAALI");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void HYLShouldBeMappedTo0()
    {
        // Arrange
        var mapper = new VotingTypeMapper();

        // Act
        var result = mapper.MapToVotingType("HYL");

        // Assert
        Assert.Equal(3, result);
    }

        [Fact]
        public void UnknowTypeShouldThrowException()
        {
            // Arrange
            var mapper = new VotingTypeMapper();

            // Act
            var exception = Record.Exception(() => mapper.MapToVotingType("UNKNOWN"));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<NotSupportedException>(exception);
        }

}