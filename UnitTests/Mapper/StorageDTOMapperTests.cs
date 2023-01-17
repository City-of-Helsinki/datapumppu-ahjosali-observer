using MeetingRoomObserver.Handler.DTOs;
using MeetingRoomObserver.Mapper;
using MeetingRoomObserver.Models;
using MeetingRoomObserver.StorageClient.DTOs;
using Moq;
using Xunit;

namespace UnitTests.Mapper
{
    public class StorageDTOMapperTests
    {

        [Fact]
        public void CaseEvent()
        {
            var eventTypeMapper = new Mock<IMeetingEventTypeMapper>();
            var voteTypeMapper = new Mock<IVoteTypeMapper>();
            var votingTypeMapper = new Mock<IVotingTypeMapper>();
            var storageMapper = new StorageDTOMapper(
                eventTypeMapper.Object, voteTypeMapper.Object, votingTypeMapper.Object);

            var meetingEventList = new MeetingEventList
            {
                State = new StateQueryDTO
                {
                    MeetingTitleFI = "Fin Title",
                    MeetingTitleSV = "Sv Title"
                },
                Events = new List<EventDTO>
                {
                    new CaseRoomEventDTO
                    {
                        EventType = EventTypeDTOConstants.Case,
                        SequenceNumber = 1,
                        Timestamp= DateTime.Now,
                        PropositionFI = "Prop FI",
                        PropositionSV = "Prop SV",
                        CaseNumber = "64",
                        Id = "1",
                        TextFI = "Text FI",
                        TextSV = "Text SV",
                        ItemNumber = "0"
                    }
                },
                MeetingID = "test",
            };

            var result = storageMapper.MapToStorageDTOs(meetingEventList);

            Assert.Single(result);

            var resultEvent = result[0] as StorageCaseEventDTO;
            Assert.NotNull(resultEvent);
            Assert.Equal("Prop FI", resultEvent.PropositionFI);
            Assert.Equal("Prop SV", resultEvent.PropositionSV);
            Assert.Equal("64", resultEvent.CaseNumber);
            Assert.Equal("0", resultEvent.ItemNumber);
            Assert.Equal("test", resultEvent.MeetingID);
        }

        [Fact]
        public void MeetingStartEvent()
        {
            var eventTypeMapper = new Mock<IMeetingEventTypeMapper>();
            var voteTypeMapper = new Mock<IVoteTypeMapper>();
            var votingTypeMapper = new Mock<IVotingTypeMapper>();
            var storageMapper = new StorageDTOMapper(
                eventTypeMapper.Object, voteTypeMapper.Object, votingTypeMapper.Object);

            var meetingEventList = new MeetingEventList
            {
                State = new StateQueryDTO
                {
                    MeetingTitleFI = "Fin Title",
                    MeetingTitleSV = "Sv Title"
                },
                Events = new List<EventDTO>
                {
                    new MeetingStartsRoomEventDTO
                    {
                        EventType = EventTypeDTOConstants.MeetingStarts,
                        SequenceNumber = 1,
                        Timestamp= DateTime.Now,
                    }
                },
                MeetingID = "test",
            };

            var result = storageMapper.MapToStorageDTOs(meetingEventList);

            Assert.Single(result);

            var resultEvent = result[0] as StorageMeetingStartedEventDTO;
            Assert.NotNull(resultEvent);
            Assert.Equal("Fin Title", resultEvent.MeetingTitleFI);
            Assert.Equal("Sv Title", resultEvent.MeetingTitleSV);
            Assert.Equal("test", resultEvent.MeetingID);
        }
    }
}
