using MeetingRoomObserver.Handler.DTOs;

namespace MeetingRoomObserver.Models
{
    /// <summary>
    /// Contains the parsed result of a raw Ahjo meeting room JSON message,
    /// including attendees, meeting state, and a list of individual events.
    /// </summary>
    public class MeetingEventList
    {
        /// <summary>
        /// The list of attendees present in the meeting room, with seat assignments.
        /// </summary>
        public AttendeesListRoomDTO AttendeesListRoom { get; set; } = new AttendeesListRoomDTO();

        /// <summary>
        /// The raw meeting identifier from the Ahjo system (e.g. "2019/21 2019-12-11 15:56:19.358").
        /// </summary>
        public string? MeetingID { get; set; }

        /// <summary>
        /// The current meeting state, including titles, case numbers, and item numbers.
        /// </summary>
        public StateQueryDTO? State { get; set; }

        /// <summary>
        /// The list of parsed meeting events extracted from the Ahjo JSON message.
        /// </summary>
        public List<EventDTO> Events { get; set; } = new List<EventDTO>();
    }
}
