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
            if (meetingEventList == null || string.IsNullOrEmpty(meetingEventList.MeetingID))
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

                AddVotingStartsEventMapper(cfg);

                AddVotingEndEventMapper(cfg);

            });
            config.AssertConfigurationIsValid();

            return config.CreateMapper();
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
                .ForMember(dest => dest.ForText, opt => opt.MapFrom(src => src.AyeTextFI))
                .ForMember(dest => dest.ForTitle, opt => opt.MapFrom(src => src.AyeTitleFI))
                .ForMember(dest => dest.AgainstText, opt => opt.MapFrom(src => src.NayTextFI))
                .ForMember(dest => dest.AgainstTitle, opt => opt.MapFrom(src => src.NayTitleFI))
                .ForMember(dest => dest.VotesFor, opt => opt.MapFrom(src => src.AyeCount))
                .ForMember(dest => dest.VotesEmpty, opt => opt.MapFrom(src => src.EmptyCount))
                .ForMember(dest => dest.VotesAbsent, opt => opt.MapFrom(src => src.AbsentCount))
                .ForMember(dest => dest.VotesAgainst, opt => opt.MapFrom(src => src.NayCount))
                .ForMember(dest => dest.VotingTypeText, opt => opt.MapFrom(src => src.VotingTypeTextFI))
                .ForMember(dest => dest.VotingType, opt => opt.MapFrom(src => _votingTypeMapper.MapToVotingType(src.VotingType)));

            mapperConfiguration.CreateMap<VoteRoomDTO, VoteStorageDTO>()
                .ForMember(dest => dest.VoterName, opt => opt.MapFrom(src => src.NameFI))
                .ForMember(dest => dest.VoteType, opt => opt.MapFrom(src => _voteTypeMapper.MapToVoteType(src.VoteType)));
        }
    }
}
