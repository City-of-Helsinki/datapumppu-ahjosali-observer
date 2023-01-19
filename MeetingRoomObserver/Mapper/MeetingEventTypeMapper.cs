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
            { EventTypeDTOConstants.Speeches, StorageEventType.Statements },
            { EventTypeDTOConstants.Case, StorageEventType.Case },
            { EventTypeDTOConstants.RollCallStarts, StorageEventType.RollCallStarted },
            { EventTypeDTOConstants.RollCallEnds, StorageEventType.RollCallEnded },
            { EventTypeDTOConstants.FloorReservation, StorageEventType.StatementReservation },
            { EventTypeDTOConstants.FloorReservationsCleared, StorageEventType.StatementReservationsCleared },
            { EventTypeDTOConstants.SpeechStarts, StorageEventType.StatementStarted },
            { EventTypeDTOConstants.SpeechEnds, StorageEventType.StatementEnded },
            { EventTypeDTOConstants.PersonArrived, StorageEventType.PersonArrived },
            { EventTypeDTOConstants.PersonLeft, StorageEventType.PersonLeft },
            { EventTypeDTOConstants.Pause, StorageEventType.Pause },
            { EventTypeDTOConstants.PauseInfo, StorageEventType.PauseInfo },
            { EventTypeDTOConstants.DiscussionStarts, StorageEventType.DiscussionStarts },
            { EventTypeDTOConstants.SpeechTimer, StorageEventType.SpeechTimer },
            { EventTypeDTOConstants.Propositions, StorageEventType.Propositions },
            { EventTypeDTOConstants.ReplyReservation, StorageEventType.ReplyReservation },
            { EventTypeDTOConstants.ReplyReservationsCleared, StorageEventType.ReplyReservationsCleared },
            { EventTypeDTOConstants.MeetingContinues, StorageEventType.MeetingContinues }
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
