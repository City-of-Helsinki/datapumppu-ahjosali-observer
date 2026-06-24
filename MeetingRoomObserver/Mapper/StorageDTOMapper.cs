using AutoMapper;
using MeetingRoomObserver.Handler.DTOs;
using MeetingRoomObserver.Models;
using MeetingRoomObserver.StorageClient;
using MeetingRoomObserver.StorageClient.DTOs;
using System.Collections.Concurrent;

namespace MeetingRoomObserver.Mapper
{
    /// <summary>
    /// Transforms parsed Ahjo meeting event lists into normalized storage DTOs.
    /// </summary>
    public interface IStorageDTOMapper
    {
        /// <summary>
        /// Maps a parsed <see cref="MeetingEventList"/> into a list of <see cref="StorageEventDTO"/> objects,
        /// resolving the storage meeting identifier via the storage REST API.
        /// </summary>
        /// <param name="meetingEventList">The parsed meeting event list from the Ahjo system.</param>
        /// <returns>A list of normalized storage event DTOs ready for publishing.</returns>
        Task<List<StorageEventDTO>> MapToStorageDTOs(MeetingEventList? meetingEventList);
    }

    /// <summary>
    /// Orchestrates the transformation of Ahjo input DTOs into storage output DTOs using AutoMapper.
    /// Configures mapping profiles for all 22 event types, resolves meeting IDs via the storage API,
    /// and caches meeting ID lookups to reduce external API calls.
    /// </summary>
    public class StorageDTOMapper : IStorageDTOMapper
    {
        private readonly ILogger<StorageDTOMapper>? _logger;
        private readonly IMeetingEventTypeMapper _meetingEventTypeMapper;
        private readonly IVotingTypeMapper _votingTypeMapper;
        private readonly IVoteTypeMapper _voteTypeMapper;
        private readonly ISpeechTypeMapper _speechTypeMapper;
        private readonly IStorageApiClient _storageApiClient;
        private readonly ConcurrentDictionary<string, string> _meetingIdMap = new ConcurrentDictionary<string, string>();
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageDTOMapper"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="meetingEventTypeMapper">Maps event type strings to storage event types.</param>
        /// <param name="voteTypeMapper">Maps vote type strings to integer values.</param>
        /// <param name="votingTypeMapper">Maps voting type strings to integer values.</param>
        /// <param name="speechTypeMapper">Maps speech type strings to integer values.</param>
        /// <param name="storageApiClient">The client used to resolve meeting IDs from the storage API.</param>
        public StorageDTOMapper(
            ILogger<StorageDTOMapper>? logger,
            IMeetingEventTypeMapper meetingEventTypeMapper,
            IVoteTypeMapper voteTypeMapper,
            IVotingTypeMapper votingTypeMapper,
            ISpeechTypeMapper speechTypeMapper,
            IStorageApiClient storageApiClient)
        {
            _logger = logger;
            _meetingEventTypeMapper = meetingEventTypeMapper;
            _votingTypeMapper = votingTypeMapper;
            _voteTypeMapper = voteTypeMapper;
            _speechTypeMapper = speechTypeMapper;
            _storageApiClient = storageApiClient;
            _mapper = CreateMapper();
        }

        public async Task<List<StorageEventDTO>> MapToStorageDTOs(MeetingEventList? meetingEventList)
        {
            if (meetingEventList == null || string.IsNullOrEmpty(meetingEventList.MeetingID) || meetingEventList.Events.Count == 0)
            {
                return new List<StorageEventDTO>();
            }
            var meetingId = await GetMeetingId(meetingEventList.MeetingID);
            _logger?.LogInformation("meetind id {0}", meetingId);

            var state = meetingEventList.State!;
            var storageEvents = new List<StorageEventDTO>();
            storageEvents.AddRange(MapInputToOutputDTO<StorageMeetingStartedEventDTO, MeetingStartsRoomEventDTO>(meetingEventList.Events));            
            storageEvents.AddRange(MapInputToOutputDTO<StorageMeetingEndedEventDTO, MeetingEndsRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageVotingStartedEventDTO, VotingStartsRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageVotingEndedEventDTO, VotingEndsRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageStatementsEventDTO, SpeechListRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageCaseEventDTO, CaseRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageRollCallStartedEventDTO, RollCallStartsRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageRollCallEndedEventDTO, RollCallEndsRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageStatementReservationEventDTO, FloorReservationRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageStatementReservationsClearedEventDTO, FloorReservationsClearedRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageStatementStartedEventDTO, SpeechStartsRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageStatementEndedEventDTO, SpeechEndsRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StoragePersonArrivedEventDTO, PersonArrivedRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StoragePersonLeftEventDTO, PersonLeftRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StoragePauseEventDTO, PauseRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StoragePauseInfoEventDTO, PauseInfoRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageDiscussionStartsEventDTO, DiscussionStartsRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageSpeechTimerEventDTO, SpeechTimerRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StoragePropositionsEventDTO, PropositionsRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageReplyReservationEventDTO, ReplyReservationRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageReplyReservationsClearedEventDTO, ReplyReservationsClearedRoomEventDTO>(meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StorageMeetingContinuesEventDTO, MeetingContinuesRoomEventDTO>(meetingEventList.Events));

            if (meetingEventList.AttendeesListRoom.Seats.Length > 0)
            {
                storageEvents.Add(_mapper.Map<StorageAttendeesEventDTO>(meetingEventList.AttendeesListRoom));
            }

            foreach (var storageEvent in storageEvents)
            {
                storageEvent.MeetingID = meetingId;
                storageEvent.SequenceNumber = state.SequenceNumber ?? 0;
                storageEvent.CaseNumber = state.CaseNumber ?? string.Empty;
                storageEvent.ItemNumber = state.ItemNumber ?? string.Empty;

                if (storageEvent is StorageMeetingStartedEventDTO startedEvent)
                {
                    startedEvent.MeetingTitleFI = state.MeetingTitleFI;
                    startedEvent.MeetingTitleSV = state.MeetingTitleSV;
                }
            }

            return storageEvents;
        }

        private async Task<string> GetMeetingId(string id)
        {
            if (_meetingIdMap.Count > 100) // Increased limit as it's singleton now
            {
                _meetingIdMap.Clear();
            }

            if (!_meetingIdMap.TryGetValue(id, out var meetingId))
            {
                var idData = id.Split('/', ' ');
                meetingId = await _storageApiClient.GetMeetingId(idData[0], idData[1]);
                meetingId = meetingId ?? "";
                _meetingIdMap.TryAdd(id, meetingId);
            }

            return meetingId;
        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<string?, StorageEventType>()
                    .ConvertUsing(src => _meetingEventTypeMapper.MapToMeetingEventType(src));

                cfg.CreateMap<EventDTO, StorageEventDTO>()
                    .ForMember(dest => dest.MeetingID, opt => opt.Ignore())
                    .ForMember(dest => dest.CaseNumber, opt => opt.Ignore())
                    .ForMember(dest => dest.ItemNumber, opt => opt.Ignore())
                    .ForMember(dest => dest.SequenceNumber, opt => opt.Ignore())
                    .IncludeAllDerived();

                cfg.CreateMap<MeetingStartsRoomEventDTO, StorageMeetingStartedEventDTO>()
                    .ForMember(dest => dest.MeetingTitleFI, opt => opt.Ignore())
                    .ForMember(dest => dest.MeetingTitleSV, opt => opt.Ignore());
                cfg.CreateMap<MeetingEndsRoomEventDTO, StorageMeetingEndedEventDTO>();
                cfg.CreateMap<MeetingContinuesRoomEventDTO, StorageMeetingContinuesEventDTO>();

                cfg.CreateMap<VotingStartsRoomEventDTO, StorageVotingStartedEventDTO>()
                    .ForMember(dest => dest.ForTextFI, opt => opt.MapFrom(src => src.AyeTextFI))
                    .ForMember(dest => dest.ForTextSV, opt => opt.MapFrom(src => src.AyeTextSV))
                    .ForMember(dest => dest.ForTitleFI, opt => opt.MapFrom(src => src.AyeTitleFI))
                    .ForMember(dest => dest.ForTitleSV, opt => opt.MapFrom(src => src.AyeTitleSV))
                    .ForMember(dest => dest.AgainstTextFI, opt => opt.MapFrom(src => src.NayTextFI))
                    .ForMember(dest => dest.AgainstTextSV, opt => opt.MapFrom(src => src.NayTextSV))
                    .ForMember(dest => dest.AgainstTitleFI, opt => opt.MapFrom(src => src.NayTitleFI))
                    .ForMember(dest => dest.AgainstTitleSV, opt => opt.MapFrom(src => src.NayTitleSV))
                    .ForMember(dest => dest.VotingNumber, opt => opt.MapFrom(src => src.Number))
                    .ForMember(dest => dest.VotingType, opt => opt.MapFrom(src => _votingTypeMapper.MapToVotingType(src.VotingType)));

                cfg.CreateMap<VotingEndsRoomEventDTO, StorageVotingEndedEventDTO>()
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
                    .ForMember(dest => dest.VotingNumber, opt => opt.MapFrom(src => src.Number))
                    .ForMember(dest => dest.VotingType, opt => opt.MapFrom(src => _votingTypeMapper.MapToVotingType(src.VotingType)));

                cfg.CreateMap<VoteRoomDTO, StorageVoteDTO>()
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.PersonFI.Split(new char[] { '/', '(' })[0].Trim()))
                    .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))
                    .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)))
                    .ForMember(dest => dest.VoteType, opt => opt.MapFrom(src => _voteTypeMapper.MapToVoteType(src.VoteType)));

                cfg.CreateMap<AttendeesListRoomDTO, StorageAttendeesEventDTO>()
                    .ForMember(dest => dest.MeetingSeats, opt => opt.MapFrom(src => src.Seats))
                    .ForMember(dest => dest.EventType, opt => opt.Ignore())
                    .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(_ => DateTime.MinValue))
                    .ForMember(dest => dest.MeetingID, opt => opt.Ignore())
                    .ForMember(dest => dest.SequenceNumber, opt => opt.Ignore())
                    .ForMember(dest => dest.CaseNumber, opt => opt.Ignore())
                    .ForMember(dest => dest.ItemNumber, opt => opt.Ignore());

                cfg.CreateMap<SeatRoomDTO, StorageMeetingSeatDTO>()
                    .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat))
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.PersonFI.Split(new char[] { '/', '(' })[0].Trim()))
                    .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))
                    .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)));

                cfg.CreateMap<CaseRoomEventDTO, StorageCaseEventDTO>()
                    .ForMember(dest => dest.CaseTextFI, opt => opt.MapFrom(src => src.TextFI))
                    .ForMember(dest => dest.CaseTextSV, opt => opt.MapFrom(src => src.TextSV))
                    .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.Id));

                cfg.CreateMap<RollCallStartsRoomEventDTO, StorageRollCallStartedEventDTO>();
                cfg.CreateMap<RollCallEndsRoomEventDTO, StorageRollCallEndedEventDTO>();

                cfg.CreateMap<SpeechListRoomEventDTO, StorageStatementsEventDTO>()
                    .ForMember(dest => dest.Statements, opt => opt.MapFrom(src => src.Speeches));

                cfg.CreateMap<SpeechRoomDTO, StorageStatementDTO>()
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.PersonFI.Split(new char[] { '/', '(' })[0].Trim()))
                    .ForMember(dest => dest.SpeechType, opt => opt.MapFrom(src => _speechTypeMapper.MapToSpeechType(src.SpeechType)))
                    .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))
                    .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)));

                cfg.CreateMap<SpeechStartsRoomEventDTO, StorageStatementStartedEventDTO>()
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.PersonFI.Split(new char[] { '/', '(' })[0].Trim()))
                    .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))
                    .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)))
                    .ForMember(dest => dest.SpeechType, opt => opt.MapFrom(src => _speechTypeMapper.MapToSpeechType(src.SpeechType)))
                    .ForMember(dest => dest.SpeakingTime, opt => opt.MapFrom(src => src.SpeechTime))
                    .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat));

                cfg.CreateMap<SpeechEndsRoomEventDTO, StorageStatementEndedEventDTO>();

                cfg.CreateMap<FloorReservationRoomEventDTO, StorageStatementReservationEventDTO>()
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.PersonFI.Split(new char[] { '/', '(' })[0].Trim()))
                    .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))
                    .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)))
                    .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat));

                cfg.CreateMap<FloorReservationsClearedRoomEventDTO, StorageStatementReservationsClearedEventDTO>();

                cfg.CreateMap<PersonArrivedRoomEventDTO, StoragePersonArrivedEventDTO>()
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.PersonFI.Split(new char[] { '/', '(' })[0].Trim()))
                    .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))
                    .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)))
                    .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat));

                cfg.CreateMap<PersonLeftRoomEventDTO, StoragePersonLeftEventDTO>()
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.PersonFI.Split(new char[] { '/', '(' })[0].Trim()))
                    .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))
                    .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)))
                    .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat));

                cfg.CreateMap<PauseRoomEventDTO, StoragePauseEventDTO>();
                cfg.CreateMap<PauseInfoRoomEventDTO, StoragePauseInfoEventDTO>();
                cfg.CreateMap<DiscussionStartsRoomEventDTO, StorageDiscussionStartsEventDTO>();

                cfg.CreateMap<SpeechTimerRoomEventDTO, StorageSpeechTimerEventDTO>()
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.PersonFI.Split(new char[] { '/', '(' })[0].Trim()))
                    .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))
                    .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)))
                    .ForMember(dest => dest.DurationSeconds, opt => opt.MapFrom(src => src.SpeechTime))
                    .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat));

                cfg.CreateMap<PropositionsRoomEventDTO, StoragePropositionsEventDTO>();

                cfg.CreateMap<PropositionRoomDTO, StoragePropositionDTO>()
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.PersonFI.Split(new char[] { '/', '(' })[0].Trim()))
                    .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))
                    .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.PropositionType))
                    .ForMember(dest => dest.TypeTextFI, opt => opt.MapFrom(src => src.PropositionTypeTextFI))
                    .ForMember(dest => dest.TypeTextSV, opt => opt.MapFrom(src => src.PropositionTypeTextSV));

                cfg.CreateMap<ReplyReservationRoomEventDTO, StorageReplyReservationEventDTO>()
                    .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat))
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.PersonFI.Split(new char[] { '/', '(' })[0].Trim()))
                    .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))
                    .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)));

                cfg.CreateMap<ReplyReservationsClearedRoomEventDTO, StorageReplyReservationsClearedEventDTO>();
            });
            config.AssertConfigurationIsValid();

            return config.CreateMapper();
        }

        private string ParseAdditionalInfo(string name)
        {
            var parts = name.Split(new char[] { '/', '(', ')' });
            return parts.Length < 2 ? "" : parts[1];
        }

        private IEnumerable<T1> MapInputToOutputDTO<T1, T2>(IEnumerable<EventDTO> values)
        {
            return values.Where(meetingEvent => meetingEvent is T2)
                .Select(meetingEvent => _mapper.Map<T1>(meetingEvent));
        }
    }
}
