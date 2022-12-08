using MeetingRoomObserver.Handler.DTOs;

namespace MeetingRoomObserver.Models
{
    public class MeetingEventList
    {
        public AttendeesListRoomDTO AttendeesListRoom { get; set; } = new AttendeesListRoomDTO();

        public string? MeetingID { get; set; }

        public StateQueryDTO? State { get; set; }

        public List<EventDTO> Events { get; set; } = new List<EventDTO>();
    }
}
