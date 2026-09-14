namespace MeetingRoomObserver.Mapper
{
    /// <summary>
    /// Maps Ahjo speech type codes to integer storage values.
    /// </summary>
    public interface ISpeechTypeMapper
    {
        /// <summary>
        /// Maps a speech type code to its integer storage value.
        /// </summary>
        /// <param name="speechType">The speech type code: "V" (reply) or "P" (statement).</param>
        /// <returns>0 for reply, 1 for statement.</returns>
        /// <exception cref="NotSupportedException">Thrown when the speech type is not recognized.</exception>
        int MapToSpeechType(string? speechType);
    }

    /// <summary>
    /// Maps Ahjo speech type codes ("V" for reply, "P" for statement) to integer storage values.
    /// Throws <see cref="NotSupportedException"/> for unknown types.
    /// </summary>
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
