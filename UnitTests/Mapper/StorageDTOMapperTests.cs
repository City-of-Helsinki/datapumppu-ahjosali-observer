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
                    new CaseEventRoomDTO
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

            var resultEvent = result[0] as CaseStorageDTO;
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

            var resultEvent = result[0] as MeetingStartedStorageDTO;
            Assert.NotNull(resultEvent);
            Assert.Equal("Fin Title", resultEvent.MeetingTitleFI);
            Assert.Equal("Sv Title", resultEvent.MeetingTitleSV);
            Assert.Equal("test", resultEvent.MeetingID);
        }

        [Fact]
        public void FloorReservationEvent()
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
                    new FloorReservationRoomEventDTO
                    {
                        EventType = EventTypeDTOConstants.FloorReservation,
                        SequenceNumber = 1,
                        Timestamp= DateTime.Now,
                        Ordinal = 64,
                        PersonFI = "Kalle /SDP",
                        PersonSV = "Kalle (pormestari)"
                    }
                },
                MeetingID = "test",
            };

            var result = storageMapper.MapToStorageDTOs(meetingEventList);

            var resultEvent = result[0] as StatementReservationStorageDTO;
            Assert.NotNull(resultEvent);
            Assert.Equal("test", resultEvent.MeetingID);
            Assert.Equal("Kalle", resultEvent.Person);
            Assert.Equal("SDP", resultEvent.AdditionalInfoFI);
            Assert.Equal("pormestari", resultEvent.AdditionalInfoSV);
            Assert.Equal(64, resultEvent.Ordinal);
        }

        [Fact]
        public void StatementListEvent()
        {
            var eventTypeMapper = new Mock<IMeetingEventTypeMapper>();
            var voteTypeMapper = new Mock<IVoteTypeMapper>();
            var votingTypeMapper = new Mock<IVotingTypeMapper>();
            var storageMapper = new StorageDTOMapper(
                eventTypeMapper.Object, voteTypeMapper.Object, votingTypeMapper.Object);

            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddDays(1);
            var speechList = new List<SpeechRoomDTO>
            {
                new SpeechRoomDTO
                {
                     Duration = 64,
                     EndTime = endTime,
                     StartTime = startTime,
                     PersonFI = "Kalle (pormestari)",
                     PersonSV = "Kalle /RKP",
                     SpeectType = "P"
                },
                new SpeechRoomDTO
                {
                     Duration = 65,
                     EndTime = endTime,
                     StartTime = startTime,
                     PersonFI = "Kalle (pormestari)",
                     PersonSV = "Kalle /RKP",
                     SpeectType = "V"
                }
            };

            var meetingEventList = new MeetingEventList
            {
                State = new StateQueryDTO
                {
                    MeetingTitleFI = "Fin Title",
                    MeetingTitleSV = "Sv Title"
                },
                Events = new List<EventDTO>
                {
                    new SpeechListRoomEventDTO
                    {
                        EventType = EventTypeDTOConstants.Speeches,
                        SequenceNumber = 1,
                        Timestamp= DateTime.Now,
                        Speeches = speechList.ToArray()
                    }
                },
                MeetingID = "test",
            };

            var result = storageMapper.MapToStorageDTOs(meetingEventList);

            var resultEvent = result[0] as StatementsStorageDTO;
            Assert.NotNull(resultEvent);

            var s1 = resultEvent.SpeakingTurns[0];
            Assert.Equal("test", resultEvent.MeetingID);
            Assert.Equal("Kalle", s1.Person);
            Assert.Equal("pormestari", s1.AdditionalInfoFI);
            Assert.Equal("RKP", s1.AdditionalInfoSV);
            Assert.Equal(64, s1.Duration);
            Assert.Equal(1, s1.SpeechType);

            var s2 = resultEvent.SpeakingTurns[1];
            Assert.Equal("Kalle", s2.Person);
            Assert.Equal("pormestari", s2.AdditionalInfoFI);
            Assert.Equal("RKP", s2.AdditionalInfoSV);
            Assert.Equal(65, s2.Duration);
            Assert.Equal(0, s2.SpeechType);
        }

        [Fact]
        public void SpeechTimerEvent()
        {
            var eventTypeMapper = new Mock<IMeetingEventTypeMapper>();
            var voteTypeMapper = new Mock<IVoteTypeMapper>();
            var votingTypeMapper = new Mock<IVotingTypeMapper>();
            var storageMapper = new StorageDTOMapper(
                eventTypeMapper.Object, voteTypeMapper.Object, votingTypeMapper.Object);

            var startTime = DateTime.Now;

            var timerEvent = new SpeechTimerRoomEventDTO
            {
                Direction = "UP",
                PersonFI = "Kalle /SDP",
                PersonSV = "Kalle /RKP",
                Seat = "64",
                SequenceNumber = 1,
                SpeechTime = 64,
                SpeechTimer = 2,
                Timestamp = startTime
            };

            var meetingEventList = new MeetingEventList
            {
                State = new StateQueryDTO
                {
                    MeetingTitleFI = "Fin Title",
                    MeetingTitleSV = "Sv Title"
                },
                Events = new List<EventDTO>
                {
                    timerEvent
                },
                MeetingID = "test",
            };

            var result = storageMapper.MapToStorageDTOs(meetingEventList);

            var resultEvent = result[0] as SpeechTimerStorageDTO;
            Assert.NotNull(resultEvent);

            Assert.Equal("test", resultEvent.MeetingID);
            Assert.Equal("Kalle", resultEvent.Person);
            Assert.Equal("SDP", resultEvent.AdditionalInfoFI);
            Assert.Equal("RKP", resultEvent.AdditionalInfoSV);
            Assert.Equal("UP", resultEvent.Direction);
            Assert.Equal(64, resultEvent.DurationSeconds);
            Assert.Equal(2, resultEvent.SpeechTimer);
            Assert.Equal("64", resultEvent.SeatID);
        }

        [Fact]
        public void RollCallEvent()
        {
            var eventTypeMapper = new Mock<IMeetingEventTypeMapper>();
            var voteTypeMapper = new Mock<IVoteTypeMapper>();
            var votingTypeMapper = new Mock<IVotingTypeMapper>();
            var storageMapper = new StorageDTOMapper(
                eventTypeMapper.Object, voteTypeMapper.Object, votingTypeMapper.Object);


            var rollCallEvent = new RollCallEndsRoomEventDTO
            {
                 Absent = 64,
                 Present = 128,
            };

            var meetingEventList = new MeetingEventList
            {
                State = new StateQueryDTO
                {
                    MeetingTitleFI = "Fin Title",
                    MeetingTitleSV = "Sv Title"
                },
                Events = new List<EventDTO>
                {
                    rollCallEvent
                },
                MeetingID = "test",
            };

            var result = storageMapper.MapToStorageDTOs(meetingEventList);

            var resultEvent = result[0] as RollCallStorageDTO;
            Assert.NotNull(resultEvent);

            Assert.Equal("test", resultEvent.MeetingID);
            Assert.Equal(64, resultEvent.Absent);
            Assert.Equal(128, resultEvent.Present);
        }

    }
}
