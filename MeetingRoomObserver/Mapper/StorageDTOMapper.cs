using AutoMapper;
using MeetingRoomObserver.Handler.DTOs;
using MeetingRoomObserver.Models;
using MeetingRoomObserver.StorageClient;
using MeetingRoomObserver.StorageClient.DTOs;
using System;

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

        public StorageDTOMapper(
            IMeetingEventTypeMapper meetingEventTypeMapper,
            IVoteTypeMapper voteTypeMapper,
            IVotingTypeMapper votingTypeMapper)
        {
            _meetingEventTypeMapper = meetingEventTypeMapper;
            _votingTypeMapper = votingTypeMapper;
            _voteTypeMapper = voteTypeMapper;
        }

        public List<StorageEventDTO> MapToStorageDTOs(MeetingEventList? meetingEventList)
        {
            if (meetingEventList == null || string.IsNullOrEmpty(meetingEventList.MeetingID) || meetingEventList.Events.Count == 0)
            {
                return new List<StorageEventDTO>();
            }

            var mapper = CreateMapper(meetingEventList.State!, meetingEventList.MeetingID);

            var storageEvents = new List<StorageEventDTO>();
            storageEvents.AddRange(MapInputToOutputDTO<MeetingStartedStorageDTO, MeetingStartsRoomEventDTO>(mapper, meetingEventList.Events));            
            storageEvents.AddRange(MapInputToOutputDTO<MeetingEndedStorageDTO, MeetingEndsRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<CaseStorageDTO, CaseEventRoomDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<VotingStartedStorageDTO, VotingStartsRoomEventDTO > (mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<VotingEndedStorageDTO, VotingEndsRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StatementReservationStorageDTO, FloorReservationRoomEventDTO>(mapper, meetingEventList.Events));
            storageEvents.AddRange(MapInputToOutputDTO<StatementsStorageDTO, SpeechListRoomEventDTO>(mapper, meetingEventList.Events));


            if (meetingEventList.AttendeesListRoom != null)
            {
                storageEvents.Add(MapAttendees(mapper, meetingEventList.AttendeesListRoom));
            }

            return storageEvents.ToList();
        }

        private IMapper CreateMapper(StateQueryDTO state, string meetingId)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<string?, StorageEventType>()
                    .ConvertUsing(src => _meetingEventTypeMapper.MapToMeetingEventType(src));

                cfg.CreateMap<MeetingStartsRoomEventDTO, MeetingStartedStorageDTO>()
                    .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                    .ForMember(dest => dest.MeetingTitleFI, opt => opt.MapFrom(_ => state.MeetingTitleFI))
                    .ForMember(dest => dest.MeetingTitleSV, opt => opt.MapFrom(_ => state.MeetingTitleSV));

                cfg.CreateMap<CaseEventRoomDTO, CaseStorageDTO>()
                    .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId));

                cfg.CreateMap<MeetingEndsRoomEventDTO, MeetingEndedStorageDTO>()
                    .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId));

                cfg.CreateMap<AttendeesListRoomDTO, AttendeesEventStorageDTO>()
                    .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                    .ForMember(dest => dest.EventType, opt => opt.MapFrom(_ => StorageEventType.Attendees))
                    .ForMember(dest => dest.MeetingSeats, opt => opt.MapFrom(src => src.Seats))
                    .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(_ => 0))
                    .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(_ => DateTime.MinValue));

                cfg.CreateMap<SeatRoomDTO, MeetingSeatStorageDTO>()
                    .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat))
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => ParsePerson(src.PersonFI)))
                    .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))
                    .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)));

                cfg.CreateMap<FloorReservationRoomEventDTO, StatementReservationStorageDTO>()
                    .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                    .ForMember(dest => dest.SeatID, opt => opt.MapFrom(src => src.Seat))
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => ParsePerson(src.PersonFI)))
                    .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))
                    .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)));

                cfg.CreateMap<SpeechListRoomEventDTO, StatementsStorageDTO>()
                    .ForMember(dest => dest.MeetingID, opt => opt.MapFrom(_ => meetingId))
                    .ForMember(dest => dest.SpeakingTurns, opt => opt.MapFrom(src => src.Speeches));

                cfg.CreateMap<SpeechRoomDTO, StatementStorageDTO>()
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => ParsePerson(src.PersonFI)))
                    .ForMember(dest => dest.SpeechType, opt => opt.MapFrom(src => src.SpeectType.ToUpper() == "P" ? 1 : 0))
                    .ForMember(dest => dest.AdditionalInfoFI, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonFI)))                    
                    .ForMember(dest => dest.AdditionalInfoSV, opt => opt.MapFrom(src => ParseAdditionalInfo(src.PersonSV)));

                AddVotingStartsEventMapper(cfg);

                AddVotingEndEventMapper(cfg);
            });
            config.AssertConfigurationIsValid();

            return config.CreateMapper();
        }

        private string ParsePerson(string person)
        {
            var name = person.Split('/', '(')[0];
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            return name.Trim();
        }

        private string ParseAdditionalInfo(string name)
        {
            var parts = name.Split('/', '(', ')');
            return parts.Length < 2 ? "" : parts[1];
        }

        private AttendeesEventStorageDTO MapAttendees(IMapper mapper, AttendeesListRoomDTO attendeesListRoomDTO)
        {
            return mapper.Map<AttendeesEventStorageDTO>(attendeesListRoomDTO);
        }

        private IEnumerable<T1> MapInputToOutputDTO<T1, T2>(IMapper mapper, IEnumerable<EventDTO> values)
        {
            return values.Where(meetingEvent => meetingEvent is T2)
                .Select(meetingEvent => mapper.Map<T1>(meetingEvent));
        }

        private void AddVotingStartsEventMapper(IMapperConfigurationExpression mapperConfiguration)
        {
            mapperConfiguration.CreateMap<VotingStartsRoomEventDTO, VotingStartedStorageDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.Ignore())
                .ForMember(dest => dest.ForText, opt => opt.MapFrom(src => src.AyeTextFI))
                .ForMember(dest => dest.ForTitle, opt => opt.MapFrom(src => src.AyeTitleFI))
                .ForMember(dest => dest.AgainstText, opt => opt.MapFrom(src => src.NayTextFI))
                .ForMember(dest => dest.AgainstTitle, opt => opt.MapFrom(src => src.NayTitleFI))
                .ForMember(dest => dest.VotingTypeText, opt => opt.MapFrom(src => src.VotingTypeTextFI))
                .ForMember(dest => dest.VotingType, opt => opt.MapFrom(src => _votingTypeMapper.MapToVotingType(src.VotingType)));
        }

        private void AddVotingEndEventMapper(IMapperConfigurationExpression mapperConfiguration)
        {
            mapperConfiguration.CreateMap<VotingEndsRoomEventDTO, VotingEndedStorageDTO>()
                .ForMember(dest => dest.MeetingID, opt => opt.Ignore())
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
                .ForMember(dest => dest.VotingType, opt => opt.MapFrom(src => _votingTypeMapper.MapToVotingType(src.VotingType)));
                

            mapperConfiguration.CreateMap<VoteRoomDTO, VoteStorageDTO>()
                .ForMember(dest => dest.VoterName, opt => opt.MapFrom(src => src.NameFI))
                .ForMember(dest => dest.VoteType, opt => opt.MapFrom(src => _voteTypeMapper.MapToVoteType(src.VoteType)));
        }
    }
}
