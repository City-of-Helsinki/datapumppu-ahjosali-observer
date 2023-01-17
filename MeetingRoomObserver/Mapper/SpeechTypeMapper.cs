namespace MeetingRoomObserver.Mapper
{
    public interface ISpeechTypeMapper
    {
        int MapToSpeechType(string? speechType);
    }

    public class SpeechTypeMapper : ISpeechTypeMapper
    {
        private readonly Dictionary<string, int> _map = new Dictionary<string, int>()
        {
            { "V", 0 },
            { "P", 1 },
        };

        public int MapToSpeechType(string? speechType)
        {
            if (speechType == null || !_map.ContainsKey(speechType))
            {
                throw new NotSupportedException("Unknown speech type: " + speechType);
            }

            return _map[speechType];
        }
    }
}
