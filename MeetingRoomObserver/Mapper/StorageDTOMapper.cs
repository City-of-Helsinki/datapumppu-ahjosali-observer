using AutoMapper;
using MeetingRoomObserver.Handler.DTOs;
using MeetingRoomObserver.Models;
using MeetingRoomObserver.StorageClient;
using MeetingRoomObserver.StorageClient.DTOs;

namespace MeetingRoomObserver.Mapper
{
    public interface IStorageDTOMapper
    {
        List<StorageEventDTO> MapToStorageDTOs(MeetingEventList? meetingEventList);
    }

    public class StorageDTOMapper : IStorageDTOMapper
    {
        private readonly IMeetingEventTypeMapper _meetingEventTypeMapper;
        private readonly IVotingTypeMapper _votingTypeMapper;
        private readonly IVoteTypeMapper _voteTypeMapper;
        private readonly ISpeechTypeMapper _speechTypeMapper;

        public StorageDTOMapper(
            IMeetingEventTypeMapper meetingEventTypeMapper,
            IVoteTypeMapper voteTypeMapper,
            IVotingTypeMapper votingTypeMapper,
            ISpeechTypeMapper speechTypeMapper)
        {
            _meetingEventTypeMapper = meetingEventTypeMapper;
            _votingTypeMapper = votingTypeMapper;
            _voteTypeMapper = voteTypeMapper;
            _speechTypeMapper = speechTypeMapper;
        }

        public List<StorageEventDTO> MapToStorageDTOs(MeetingEventList? meetingEventList)
        {
            if (meetingEventList == null || string.IsNullOrEmpty(meetingEventList.MeetingID) || meetingEventList.Events.Count == 0)
            {
                return new List<StorageEventDTO>();
            }
            var meetingId = FormatMeetingId(meetingEventList.MeetingID);
            var mapper = CreateMapper(meetingEventList.State!, meetingId);

            var storageEvents = new List<StorageEventDTO>();
            storageEvents.AddRange(MapInputToOutputDTO<StorageMeetingStartedEventDTO, MeetingStartsRoomEventDTO>(mapper, meetingEventList.Events));            
            storageEvents.AddRange(MapInputToOutputDTO<StorageMeetingEndedEventDTO, MeetingEndsRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageVotingStartedEventDTO, VotingStartsRoomEventDTO > (mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageVotingEndedEventDTO, VotingEndsRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageSpeakingTurnsEventDTO, SpeechListRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageCaseEventDTO, CaseRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageRollCallStartedEventDTO, RollCallStartsRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageRollCallEndedEventDTO, RollCallEndsRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageSpeakingTurnReservationEventDTO, FloorReservationRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageSpeakingTurnReservationsEmptiedEventDTO, FloorReservationsClearedRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageSpeakingTurnStartedEventDTO, SpeechStartsRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageSpeakingTurnEndedEventDTO, SpeechEndsRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StoragePersonArrivedEventDTO, PersonArrivedRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StoragePersonLeftEventDTO, PersonLeftRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageBreakEventDTO, PauseRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageBreakNoticeEventDTO, PauseInfoRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageDiscussionStartsEventDTO, DiscussionStartsRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageSpeechTimerEventDTO, SpeechTimerRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StoragePropositionsEventDTO, PropositionsRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageReplyReservationEventDTO, ReplyReservationRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageReplyReservationsEmptiedEventDTO, ReplyReservationsClearedRoomEventDTO>(mapper, meetingEventList.Events));

            if (meetingEventList.AttendeesListRoom != null)
            {
                storageEvents.Add(MapAttendees(mapper, meetingEventList.AttendeesListRoom));
            }

            return storageEvents.ToList();
        }

        private string FormatMeetingId(string id)
        {
            const string DecisionMaker = "02900";
            var idData = id.Split('/', ' ');
            return $"{DecisionMaker}{idData[0]}{idData[1]}";
        }

        private IMapper CreateMapper(StateQueryDTO state, string meetingId)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<string?, StorageEventType>()
                    .ConvertUsing(src => _meetingEventTypeMapper.MapToMeetingEventType(src));

                AddStorageMeetingStatusEventMappers(cfg, state, meetingId);

                AddStorageVotingEventMappers(cfg, state, meetingId);

                AddStorageAttendeesEventMappers(cfg, state, meetingId);

                AddStorageCaseEventMapper(cfg, state, meetingId);

                AddStorageRollCallEventMappers(cfg, state, meetingId);

                AddStorageSpeakingTurnEventMappers(cfg, state, meetingId);

                AddStoragePersonEventMappers(cfg, state, meetingId);

                AddStorageBreakEventMappers(cfg, state, meetingId);

                AddStorageDiscussionStartsEventMapper(cfg, state, meetingId);

                AddStorageSpeechTimerEventMapper(cfg, state, meetingId);

                AddStoragePropositionsEventMapper(cfg, state, meetingId);

                AddStorageReplyReservationEventMappers(cfg, state, meetingId);
            });
         //   config.AssertConfigurationIsValid();

            return config.CreateMapper();
        }

        private string ParseAdditionalInfo(string name)
        {
            var parts = name.Split('/', '(', ')');
            return parts.Length < 2 ? "" : parts[1];
        }

        private StorageAttendeesEventDTO MapAttendees(IMapper mapper, AttendeesListRoomDTO attendeesListRoomDTO)
        {
            return mapper.Map<StorageAttendeesEventDTO>(attendeesListRoomDTO);
        }

        private IEnumerable<T1> MapInputToOutputDTO<T1, T2>(IMapper mapper, IEnumerable<EventDTO> values)
        {
            return values.Where(meetingEvent => meetingEvent is T2)
                .Select(meetingEvent => mapper.Map<T1>(meetingEvent));
        }

        private void AddStorageMeetingStatusEventMappers(IMapperConfigurationExpression mapperConfiguration, StateQueryDTO state, string meetingId)
        {
            mapperConfiguration.CreateMap<MeetingStartsRoomEventDTO, StorageMeetingStartedEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.MeetingTitleFI, opt => opt.MapFrom(_ => state.MeetingTitleFI))
                .ForMember(dest => dest.MeetingTitleSV, opt => opt.MapFrom(_ => state.MeetingTitleSV))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber));

            mapperConfiguration.CreateMap<MeetingEndsRoomEventDTO, StorageMeetingEndedEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber));
        }

        private void AddStorageVotingEventMappers(IMapperConfigurationExpression mapperConfiguration, StateQueryDTO state, string meetingId)
        {
            mapperConfiguration.CreateMap<VotingStartsRoomEventDTO, StorageVotingStartedEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.ForTextFI, opt => opt.MapFrom(src => src.AyeTextFI))
                .ForMember(dest => dest.ForTextSV, opt => opt.MapFrom(src => src.AyeTextSV))
                .ForMember(dest => dest.ForTitleFI, opt => opt.MapFrom(src => src.AyeTitleFI))
                .ForMember(dest => dest.ForTitleSV, opt => opt.MapFrom(src => src.AyeTitleSV))
                .ForMember(dest => dest.AgainstTextFI, opt => opt.MapFrom(src => src.NayTextFI))
                .ForMember(dest => dest.AgainstTextSV, opt => opt.MapFrom(src => src.NayTextSV))
                .ForMember(dest => dest.AgainstTitleFI, opt => opt.MapFrom(src => src.NayTitleFI))
                .ForMember(dest => dest.AgainstTitleSV, opt => opt.MapFrom(src => src.NayTitleSV))
                .ForMember(dest => dest.VotingTypeTextFI, opt => opt.MapFrom(src => src.VotingTypeTextFI))
                .ForMember(dest => dest.VotingTypeTextSV, opt => opt.MapFrom(src => src.VotingTypeTextSV))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber))
                .ForMember(dest => dest.VotingNumber, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.VotingType, opt => opt.MapFrom(src => _votingTypeMapper.MapToVotingType(src.VotingType)));

            mapperConfiguration.CreateMap<VotingEndsRoomEventDTO, StorageVotingEndedEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.ForTextFI, opt => opt.MapFrom(src => src.AyeTextFI))
                .ForMember(dest => dest.ForTitleFI, opt => opt.MapFrom(src => src.AyeTitleFI))
                .ForMember(dest => dest.AgainstTextFI, opt => opt.MapFrom(src => src.NayTextFI))
                .ForMember(dest => dest.AgainstTitleFI, opt => opt.MapFrom(src => src.NayTitleFI))
                .ForMember(dest => dest.ForTextSV, opt => opt.MapFrom(src => src.AyeTextSV))
                .ForMember(dest => dest.ForTitleSV, opt => opt.MapFrom(src => src.AyeTitleSV))
                .ForMember(dest => dest.AgainstTextSV, opt => opt.MapFrom(src => src.NayTextSV))
                .ForMember(dest => dest.AgainstTitleSV, opt => opt.MapFrom(src => src.NayTitleSV))
                .ForMember(dest => dest.VotesFor, opt => opt.MapFrom(src => src.AyeCount))
                .ForMember(dest => dest.VotesEmpty, opt => opt.MapFrom(src => src.EmptyCount))
                .ForMember(dest => dest.VotesAbsent, opt => opt.MapFrom(src => src.AbsentCount))
                .ForMember(dest => dest.VotesAgainst, opt => opt.MapFrom(src => src.NayCount))
                .ForMember(dest => dest.VotingTypeTextFI, opt => opt.MapFrom(src => src.VotingTypeTextFI))
                .ForMember(dest => dest.VotingTypeTextSV, opt => opt.MapFrom(src => src.VotingTypeTextSV))
                .ForMember(dest => dest.VotingNumber, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber))
                .ForMember(dest => dest.VotingType, opt => opt.MapFrom(src => _votingTypeMapper.MapToVotingType(src.VotingType)));
                

            mapperConfiguration.CreateMap<VoteRoomDTO, StorageVoteDTO>()
                .ForMember(dest => dest.VoterName, opt => opt.MapFrom(src => src.NameFI))
                .ForMember(dest => dest.VoteType, opt => opt.MapFrom(src => _voteTypeMapper.MapToVoteType(src.VoteType)));
        }

        private void AddStorageAttendeesEventMappers(IMapperConfigurationExpression mapperConfiguration, StateQueryDTO state, string meetingId)
        {
            mapperConfiguration.CreateMap<AttendeesListRoomDTO, StorageAttendeesEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.MeetingSeats, opt => opt.MapFrom(src => src.Seats))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(_ => DateTime.MinValue));

            mapperConfiguration.CreateMap<SeatRoomDTO, StorageMeetingSeatDTO>()
                .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat))
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.PersonFI.Split('/', '(')[0]))
                .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))
                .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)));
        }

        private void AddStorageCaseEventMapper(IMapperConfigurationExpression mapperConfiguration, StateQueryDTO state, string meetingId)
        {
            mapperConfiguration.CreateMap<CaseRoomEventDTO, StorageCaseEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber))
                .ForMember(dest => dest.CaseTextFI, opt => opt.MapFrom(src => src.TextFI))
                .ForMember(dest => dest.CaseTextSV, opt => opt.MapFrom(src => src.TextSV))
                .ForMember(dest => dest.PropositionFI, opt => opt.MapFrom(src => src.PropositionFI))
                .ForMember(dest => dest.PropositionSV, opt => opt.MapFrom(src => src.PropositionSV))
                .ForMember(dest => dest.ItemTextFI, opt => opt.MapFrom(src => src.ItemTextFI))
                .ForMember(dest => dest.ItemTextSV, opt => opt.MapFrom(src => src.ItemTextSV))
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Id));
        }

        private void AddStorageRollCallEventMappers(IMapperConfigurationExpression mapperConfiguration, StateQueryDTO state, string meetingId)
        {
            mapperConfiguration.CreateMap<RollCallStartsRoomEventDTO, StorageRollCallStartedEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber));

            mapperConfiguration.CreateMap<RollCallEndsRoomEventDTO, StorageRollCallEndedEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber));
        }

        private void AddStorageSpeakingTurnEventMappers(IMapperConfigurationExpression mapperConfiguration, StateQueryDTO state, string meetingId)
        {
            mapperConfiguration.CreateMap<SpeechListRoomEventDTO, StorageSpeakingTurnsEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SpeakingTurns, opt => opt.MapFrom(src => src.Speeches))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber));

            mapperConfiguration.CreateMap<SpeechRoomDTO, StorageSpeakingTurnDTO>()
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.PersonFI.Split('/', '(')[0]))
                .ForMember(dest => dest.SpeechType, opt => opt.MapFrom(src => _speechTypeMapper.MapToSpeechType(src.SpeechType)))
                .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))
                .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)));

            mapperConfiguration.CreateMap<SpeechStartsRoomEventDTO, StorageSpeakingTurnStartedEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber))
                .ForMember(dest => dest.PersonFI, opt => opt.MapFrom(src => src.PersonFI.Split('/', '(')[0]))
                .ForMember(dest => dest.PersonSV, opt => opt.MapFrom(src => src.PersonSV))
                .ForMember(dest => dest.SpeechType, opt => opt.MapFrom(src => _speechTypeMapper.MapToSpeechType(src.SpeechType)))
                .ForMember(dest => dest.SpeakingTime, opt => opt.MapFrom(src => src.SpeechTime))
                .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat));

            mapperConfiguration.CreateMap<SpeechEndsRoomEventDTO, StorageSpeakingTurnEndedEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber));

            mapperConfiguration.CreateMap<FloorReservationRoomEventDTO, StorageSpeakingTurnReservationEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber))
                .ForMember(dest => dest.PersonFI, opt => opt.MapFrom(src => src.PersonFI.Split('/', '(')[0]))
                .ForMember(dest => dest.PersonSV, opt => opt.MapFrom(src => src.PersonSV))
                .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat));

            mapperConfiguration.CreateMap<FloorReservationsClearedRoomEventDTO, StorageSpeakingTurnReservationsEmptiedEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber));
        }

        private void AddStoragePersonEventMappers(IMapperConfigurationExpression mapperConfiguration, StateQueryDTO state, string meetingId)
        {
            mapperConfiguration.CreateMap<PersonArrivedRoomEventDTO, StoragePersonArrivedEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber))
                .ForMember(dest => dest.PersonFI, opt => opt.MapFrom(src => src.PersonFI.Split('/', '(')[0]))
                .ForMember(dest => dest.PersonSV, opt => opt.MapFrom(src => src.PersonSV))
                .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat));

            mapperConfiguration.CreateMap<PersonLeftRoomEventDTO, StoragePersonLeftEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber))
                .ForMember(dest => dest.PersonFI, opt => opt.MapFrom(src => src.PersonFI.Split('/', '(')[0]))
                .ForMember(dest => dest.PersonSV, opt => opt.MapFrom(src => src.PersonSV))
                .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat));
        }

        private void AddStorageBreakEventMappers(IMapperConfigurationExpression mapperConfiguration, StateQueryDTO state, string meetingId)
        {
            mapperConfiguration.CreateMap<PauseRoomEventDTO, StorageBreakEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber));

            mapperConfiguration.CreateMap<PauseInfoRoomEventDTO, StorageBreakNoticeEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber))
                .ForMember(dest => dest.Notice, opt => opt.MapFrom(src => src.Info));
        }

        private void AddStorageDiscussionStartsEventMapper(IMapperConfigurationExpression mapperConfiguration, StateQueryDTO state, string meetingId)
        {
            mapperConfiguration.CreateMap<DiscussionStartsRoomEventDTO, StorageDiscussionStartsEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber));
        }

        private void AddStorageSpeechTimerEventMapper(IMapperConfigurationExpression mapperConfiguration, StateQueryDTO state, string meetingId)
        {
            mapperConfiguration.CreateMap<SpeechTimerRoomEventDTO, StorageSpeechTimerEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.PersonFI, opt => opt.MapFrom(src => src.PersonFI.Split('/', '(')[0]))
                .ForMember(dest => dest.PersonSV, opt => opt.MapFrom(src => src.PersonSV))
                .ForMember(dest => dest.DurationSeconds, opt => opt.MapFrom(src => src.SpeechTime))
                .ForMember(dest => dest.SpeechTimer, opt => opt.MapFrom(src => src.SpeechTimer))
                .ForMember(dest => dest.Direction, opt => opt.MapFrom(src => src.Direction))
                .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber));
        }

        private void AddStoragePropositionsEventMapper(IMapperConfigurationExpression mapperConfiguration, StateQueryDTO state, string meetingId)
        {
            mapperConfiguration.CreateMap<PropositionsRoomEventDTO, StoragePropositionsEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber));


            mapperConfiguration.CreateMap<PropositionRoomDTO, StoragePropositionDTO>()
                .ForMember(dest => dest.TextFI, opt => opt.MapFrom(src => src.TextFI))
                .ForMember(dest => dest.TextSV, opt => opt.MapFrom(src => src.TextSV))
                .ForMember(dest => dest.PersonFI, opt => opt.MapFrom(src => src.PersonFI.Split('/', '(')[0]))
                .ForMember(dest => dest.PersonSV, opt => opt.MapFrom(src => src.PersonSV))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.PropositionType))
                .ForMember(dest => dest.TypeTextFI, opt => opt.MapFrom(src => src.PropositionTypeTextFI))
                .ForMember(dest => dest.TypeTextSV, opt => opt.MapFrom(src => src.PropositionTypeTextSV));      
        }

        private void AddStorageReplyReservationEventMappers(IMapperConfigurationExpression mapperConfiguration, StateQueryDTO state, string meetingId)
        {
            mapperConfiguration.CreateMap<ReplyReservationRoomEventDTO, StorageReplyReservationEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber))
                .ForMember(dest => dest.PersonFI, opt => opt.MapFrom(src => src.PersonFI.Split('/', '(')[0]))
                .ForMember(dest => dest.PersonSV, opt => opt.MapFrom(src => src.PersonSV));

            mapperConfiguration.CreateMap<ReplyReservationsClearedRoomEventDTO, StorageReplyReservationsEmptiedEventDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => state.SequenceNumber))
                .ForMember(dest => dest.CaseNumber, opt => opt.MapFrom(_ => state.CaseNumber))
                .ForMember(dest => dest.ItemNumber, opt => opt.MapFrom(_ => state.ItemNumber));
        }

}
}
