using Xunit;
using MeetingRoomObserver.Mapper;

namespace UnitTests.Mapper
{
    public class VoteTypeMapperTests
    {
        [Fact]
        public void UnknowTypeShouldThrowException()
        {
            // Arrange
            var mapper = new VoteTypeMapper();

            // Act
            var exception = Record.Exception(() => mapper.MapToVoteType("UNKNOWN"));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<NotSupportedException>(exception);
        }

        [Fact]
        public void ValueEIShouldReturn1()
        {
            // Arrange
            var mapper = new VoteTypeMapper();

            // Act
            var voteType = mapper.MapToVoteType("EI");

            // Assert
            Assert.Equal(1, voteType);
        }
    }
}

