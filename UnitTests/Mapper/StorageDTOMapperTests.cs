using Castle.Core.Logging;
using MeetingRoomObserver.Handler.DTOs;
using MeetingRoomObserver.Mapper;
using MeetingRoomObserver.Models;
using MeetingRoomObserver.StorageClient;
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
            var speechTypeMapper = new Mock<ISpeechTypeMapper>();
            var storageApiClient = new Mock<IStorageApiClient>();
            storageApiClient.Setup(m => m.GetMeetingId(It.IsAny<string>(), It.IsAny<string>())).Returns(() => Task.FromResult("02900201521"));

            var storageMapper = new StorageDTOMapper(null,
                eventTypeMapper.Object, voteTypeMapper.Object, votingTypeMapper.Object, speechTypeMapper.Object, storageApiClient.Object);

            var meetingEventList = new MeetingEventList
            {
                State = new StateQueryDTO
                {
                    MeetingTitleFI = "Fin Title",
                    MeetingTitleSV = "Sv Title",
                    CaseNumber = "64",
                    ItemNumber = "2"
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
                        Id = "1",
                        TextFI = "Text FI",
                        TextSV = "Text SV",
                    }
                },
                MeetingID = "2015/21 2019-12-11 15:56:19.358",
            };

            var result = storageMapper.MapToStorageDTOs(meetingEventList).Result;

            Assert.Single(result);

            var resultEvent = result[0] as StorageCaseEventDTO;
            Assert.NotNull(resultEvent);
            Assert.Equal("Prop FI", resultEvent.PropositionFI);
            Assert.Equal("Prop SV", resultEvent.PropositionSV);
            Assert.Equal("64", resultEvent.CaseNumber);
            Assert.Equal("2", resultEvent.ItemNumber);
            Assert.Equal("02900201521", resultEvent.MeetingID);
        }

        [Fact]
        public void MeetingStartEvent()
        {
            var eventTypeMapper = new Mock<IMeetingEventTypeMapper>();
            var voteTypeMapper = new Mock<IVoteTypeMapper>();
            var votingTypeMapper = new Mock<IVotingTypeMapper>();
            var speechTypeMapper = new Mock<ISpeechTypeMapper>();
            var storageApiClient = new Mock<IStorageApiClient>();
            storageApiClient.Setup(m => m.GetMeetingId(It.IsAny<string>(), It.IsAny<string>())).Returns(() => Task.FromResult("02900201521"));

            var storageMapper = new StorageDTOMapper(null,
                eventTypeMapper.Object, voteTypeMapper.Object, votingTypeMapper.Object, speechTypeMapper.Object, storageApiClient.Object);

            var meetingEventList = new MeetingEventList
            {
                State = new StateQueryDTO
                {
                    MeetingTitleFI = "Fin Title",
                    MeetingTitleSV = "Sv Title",
                    CaseNumber = "2",
                    ItemNumber = "3",
                    SequenceNumber = 43423423

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
                MeetingID = "2015/21 2019-12-11 15:56:19.358",
            };

            var result = storageMapper.MapToStorageDTOs(meetingEventList).Result;

            Assert.Single(result);

            var resultEvent = result[0] as StorageMeetingStartedEventDTO;
            Assert.NotNull(resultEvent);
            Assert.Equal("Fin Title", resultEvent.MeetingTitleFI);
            Assert.Equal("Sv Title", resultEvent.MeetingTitleSV);
            Assert.Equal("02900201521", resultEvent.MeetingID);
        }

        [Fact]
        public void FloorReservationEvent()
        {
            var eventTypeMapper = new Mock<IMeetingEventTypeMapper>();
            var voteTypeMapper = new Mock<IVoteTypeMapper>();
            var votingTypeMapper = new Mock<IVotingTypeMapper>();
            var speechTypeMapper = new Mock<ISpeechTypeMapper>();
            var storageApiClient = new Mock<IStorageApiClient>();
            storageApiClient.Setup(m => m.GetMeetingId(It.IsAny<string>(), It.IsAny<string>())).Returns(() => Task.FromResult("02900201521"));

            var storageMapper = new StorageDTOMapper(null,
                eventTypeMapper.Object, voteTypeMapper.Object, votingTypeMapper.Object, speechTypeMapper.Object, storageApiClient.Object);

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
                MeetingID = "2015/21 2019-12-11 15:56:19.358",
            };

            var result = storageMapper.MapToStorageDTOs(meetingEventList).Result;

            var resultEvent = result[0] as StorageStatementReservationEventDTO;
            Assert.NotNull(resultEvent);
            Assert.Equal("02900201521", resultEvent.MeetingID);
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
            var speechTypeMapper = new Mock<ISpeechTypeMapper>();
            var storageApiClient = new Mock<IStorageApiClient>();
            storageApiClient.Setup(m => m.GetMeetingId(It.IsAny<string>(), It.IsAny<string>())).Returns(() => Task.FromResult("02900201521"));

            var storageMapper = new StorageDTOMapper(null,
                eventTypeMapper.Object, voteTypeMapper.Object, votingTypeMapper.Object, speechTypeMapper.Object, storageApiClient.Object);

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
                     SpeechType = "P",
                },
                new SpeechRoomDTO
                {
                     Duration = 65,
                     EndTime = endTime,
                     StartTime = startTime,
                     PersonFI = "Kalle (pormestari)",
                     PersonSV = "Kalle /RKP",
                     SpeechType = "V"
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
                MeetingID = "2015/21 2019-12-11 15:56:19.358",
            };

            var result = storageMapper.MapToStorageDTOs(meetingEventList).Result;

            var resultEvent = result[0] as StorageStatementsEventDTO;
            Assert.NotNull(resultEvent);

            var s1 = resultEvent.Statements[0];
            Assert.Equal("02900201521", resultEvent.MeetingID);
            Assert.Equal("Kalle", s1.Person);
            Assert.Equal("pormestari", s1.AdditionalInfoFI);
            Assert.Equal("RKP", s1.AdditionalInfoSV);
            Assert.Equal(64, s1.Duration);

            var s2 = resultEvent.Statements[1];
            Assert.Equal("Kalle", s2.Person);
            Assert.Equal("pormestari", s2.AdditionalInfoFI);
            Assert.Equal("RKP", s2.AdditionalInfoSV);
            Assert.Equal(65, s2.Duration);

            speechTypeMapper.Verify(x => x.MapToSpeechType(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void SpeechTimerEvent()
        {
            var eventTypeMapper = new Mock<IMeetingEventTypeMapper>();
            var voteTypeMapper = new Mock<IVoteTypeMapper>();
            var votingTypeMapper = new Mock<IVotingTypeMapper>();
            var speechTypeMapper = new Mock<ISpeechTypeMapper>();
            var storageApiClient = new Mock<IStorageApiClient>();
            storageApiClient.Setup(m => m.GetMeetingId(It.IsAny<string>(), It.IsAny<string>())).Returns(() => Task.FromResult("02900201521"));

            var storageMapper = new StorageDTOMapper(null,
                eventTypeMapper.Object, voteTypeMapper.Object, votingTypeMapper.Object, speechTypeMapper.Object, storageApiClient.Object);

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
                MeetingID = "2015/21 2019-12-11 15:56:19.358",
            };

            var result = storageMapper.MapToStorageDTOs(meetingEventList).Result;

            var resultEvent = result[0] as StorageSpeechTimerEventDTO;
            Assert.NotNull(resultEvent);

            Assert.Equal("02900201521", resultEvent.MeetingID);
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
            var speechTypeMapper = new Mock<ISpeechTypeMapper>();
            var storageApiClient = new Mock<IStorageApiClient>();
            storageApiClient.Setup(m => m.GetMeetingId(It.IsAny<string>(), It.IsAny<string>())).Returns(() => Task.FromResult("02900201521"));

            var storageMapper = new StorageDTOMapper(null,
                eventTypeMapper.Object, voteTypeMapper.Object, votingTypeMapper.Object, speechTypeMapper.Object, storageApiClient.Object);


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
                MeetingID = "2015/21 2019-12-11 15:56:19.358",
            };

            var result = storageMapper.MapToStorageDTOs(meetingEventList).Result;

            var resultEvent = result[0] as StorageRollCallEndedEventDTO;
            Assert.NotNull(resultEvent);

            Assert.Equal("02900201521", resultEvent.MeetingID);
            Assert.Equal(64, resultEvent.Absent);
            Assert.Equal(128, resultEvent.Present);
        }

    }
}


