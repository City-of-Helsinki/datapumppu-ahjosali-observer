using MeetingRoomObserver.Handler.DTOs;
using MeetingRoomObserver.Models;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace MeetingRoomObserver.Handler
{
    public interface IMeetingEventParser
    {
        MeetingEventList ParseJsonMessage(string jsonMessage);
    }

    public class MeetingEventParser : IMeetingEventParser
    {
        private Dictionary<string, Func<dynamic, EventDTO>> _eventFactory = new Dictionary<string, Func<dynamic, EventDTO>>
        {
            { EventTypeDTOConstants.MeetingStarts, (dynamic o) => o.ToObject<MeetingStartsRoomEventDTO>() },
            { EventTypeDTOConstants.MeetingEnds, (dynamic o) => o.ToObject<MeetingEndsRoomEventDTO>() },
            { EventTypeDTOConstants.Case, (dynamic o) => o.ToObject<CaseRoomEventDTO>() },
            { EventTypeDTOConstants.RollCallStarts, (dynamic o) => o.ToObject<RollCallStartsRoomEventDTO>() },
            { EventTypeDTOConstants.RollCallEnds, (dynamic o) => o.ToObject<RollCallEndsRoomEventDTO>() },
            { EventTypeDTOConstants.VotingStarts, (dynamic o) => o.ToObject<VotingStartsRoomEventDTO>() },
            { EventTypeDTOConstants.VotingEnds, (dynamic o) => o.ToObject<VotingEndsRoomEventDTO>() },
            { EventTypeDTOConstants.PersonArrived, (dynamic o) => o.ToObject<PersonArrivedRoomEventDTO>() },
            { EventTypeDTOConstants.PersonLeft, (dynamic o) => o.ToObject<PersonLeftRoomEventDTO>() },
            { EventTypeDTOConstants.DiscussionStarts, (dynamic o) => o.ToObject<DiscussionStartsRoomEventDTO>() },
            { EventTypeDTOConstants.FloorReservationsCleared, (dynamic o) => o.ToObject<FloorReservationsClearedRoomEventDTO>() },
            { EventTypeDTOConstants.ReplyReservationsCleared, (dynamic o) => o.ToObject<ReplyReservationsClearedRoomEventDTO>() },
            { EventTypeDTOConstants.FloorReservation, (dynamic o) => o.ToObject<FloorReservationRoomEventDTO>() },
            { EventTypeDTOConstants.SpeechStarts, (dynamic o) => o.ToObject<SpeechStartsRoomEventDTO>() },
            { EventTypeDTOConstants.SpeechEnds, (dynamic o) => o.ToObject<SpeechEndsRoomEventDTO>() },
            { EventTypeDTOConstants.Speeches, (dynamic o) => o.ToObject<SpeechListRoomEventDTO>() },
            { EventTypeDTOConstants.ReplyReservation, (dynamic o) => o.ToObject<ReplyReservationRoomEventDTO>() },
            { EventTypeDTOConstants.SpeechTimer, (dynamic o) => o.ToObject<SpeechTimerRoomEventDTO>() },
            { EventTypeDTOConstants.Propositions, (dynamic o) => o.ToObject<PropositionsRoomEventDTO>() },
            { EventTypeDTOConstants.Pause, (dynamic o) => o.ToObject<PauseRoomEventDTO>() },
            { EventTypeDTOConstants.PauseInfo, (dynamic o) => o.ToObject<PauseInfoRoomEventDTO>() },

        };

        public MeetingEventList ParseJsonMessage(string jsonMessage)
        {
            jsonMessage = Uri.UnescapeDataString(jsonMessage);
            jsonMessage = RemoveDataHeader(jsonMessage);

            var events = JsonConvert.DeserializeObject<dynamic>(jsonMessage);

            if (events == null)
            {
                return new MeetingEventList();
            }

            var eventList = events.tapahtumalista;
            if (eventList == null)
            {
                return new MeetingEventList();
            }

            string meetingId = eventList.kokous.ToString();
            List<EventDTO> parsedEvents = ParseEventList(eventList);
            return new MeetingEventList
            {
                AttendeesListRoom = GetAttendees(events.lasnaolijat),
                State = GetStateQuery(events),
                MeetingID = meetingId,
                Events = parsedEvents.Where(meetinEvent => meetinEvent != null).ToList()
            };
        }

        private AttendeesListRoomDTO GetAttendees(dynamic attendeesList)
        {
            if (attendeesList == null)
            {
                return new AttendeesListRoomDTO();
            }

            return attendeesList.ToObject<AttendeesListRoomDTO>();
        }

        private StateQueryDTO GetStateQuery(dynamic events)
        {
            var stateQuery = events?.tilakysely;
            if (stateQuery == null)
            {
                throw new Exception("Tilakysely field missing");
            }

            return stateQuery.ToObject<StateQueryDTO>();
        }

        private List<EventDTO> ParseEventList(dynamic eventList)
        {
            var resultList = new List<EventDTO>();
            foreach (var meetingEvent in eventList.tapahtumat)
            {
                resultList.Add(ToMeetingEvent(meetingEvent));
            }

            return resultList;
        }

        private EventDTO? ToMeetingEvent(dynamic meetingEvent)
        {
            if (meetingEvent == null || !_eventFactory.ContainsKey(meetingEvent!.laji.ToString()))
            {
                return null;
            }

            return _eventFactory[meetingEvent!.laji.ToString()](meetingEvent);
        }

        private string RemoveDataHeader(string data) => data.Replace("data=", "");
    }
}
