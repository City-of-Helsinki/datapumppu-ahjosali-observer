using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    /// <summary>
    /// Represents the list of attendees present in the meeting room, deserialized from the Ahjo JSON payload.
    /// </summary>
    public class AttendeesListRoomDTO
    {
        [JsonProperty("paikat")]
        public SeatRoomDTO[] Seats { get; set; } = new SeatRoomDTO[0];
    }
}
