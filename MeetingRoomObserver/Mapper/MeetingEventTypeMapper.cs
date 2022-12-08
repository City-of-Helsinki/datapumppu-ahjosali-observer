using AutoMapper;
using MeetingRoomObserver.Handler.DTOs;
using MeetingRoomObserver.Models;
using MeetingRoomObserver.StorageClient;

namespace MeetingRoomObserver.Mapper
{
    public interface IMeetingEventTypeMapper
    {
        StorageEventType MapToMeetingEventType(string? meetingEventType);
    }

    public class MeetingEventTypeMapper : IMeetingEventTypeMapper
    {
        private readonly Dictionary<string, StorageEventType> _map = new Dictionary<string, StorageEventType>()
        {
            { EventTypeDTOConstants.MeetingStarts, StorageEventType.MeetingStarted },
            { EventTypeDTOConstants.MeetingEnds, StorageEventType.MeetingEnded },
            { EventTypeDTOConstants.VotingStarts, StorageEventType.VotingStarted },
            { EventTypeDTOConstants.VotingEnds, StorageEventType.VotingEnded },
        };

        public StorageEventType MapToMeetingEventType(string? meetingEventType)
        {
            if (meetingEventType == null || !_map.ContainsKey(meetingEventType))
            {
                throw new NotSupportedException("Unknown event type: " + meetingEventType);
            }

            return _map[meetingEventType];
        }
    }
}
