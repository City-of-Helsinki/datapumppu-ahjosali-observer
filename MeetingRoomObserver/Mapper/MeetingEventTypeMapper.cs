using AutoMapper;
using MeetingRoomObserver.Handler.DTOs;
using MeetingRoomObserver.Models;
using MeetingRoomObserver.StorageClient;

namespace MeetingRoomObserver.Mapper
{
    /// <summary>
    /// Maps Ahjo event type strings to <see cref="StorageEventType"/> enum values.
    /// </summary>
    public interface IMeetingEventTypeMapper
    {
        /// <summary>
        /// Maps a Finnish event type string to its corresponding <see cref="StorageEventType"/>.
        /// </summary>
        /// <param name="meetingEventType">The Ahjo event type string (e.g. "kokous alkaa").</param>
        /// <returns>The corresponding <see cref="StorageEventType"/> enum value.</returns>
        /// <exception cref="NotSupportedException">Thrown when the event type is not recognized.</exception>
        StorageEventType MapToMeetingEventType(string? meetingEventType);
    }

    /// <summary>
    /// Maps 22 Finnish Ahjo event type strings to <see cref="StorageEventType"/> enum values
    /// using a dictionary lookup. Throws <see cref="NotSupportedException"/> for unknown types.
    /// </summary>
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
