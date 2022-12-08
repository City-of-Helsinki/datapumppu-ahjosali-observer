using Newtonsoft.Json;

namespace MeetingRoomObserver.Handler.DTOs
{
    public class AttendeesListRoomDTO
    {
        [JsonProperty("paikat")]
        public SeatRoomDTO[] Seats { get; set; } = new SeatRoomDTO[0];
    }
}
