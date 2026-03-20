namespace MeetingRoomObserver.Mapper
{
    /// <summary>
    /// Maps Ahjo voting type codes to integer storage values.
    /// </summary>
    public interface IVotingTypeMapper
    {
        /// <summary>
        /// Maps a voting type code to its integer storage value (case-insensitive).
        /// </summary>
        /// <param name="voteType">The voting type code: NORMAL/NORMAALI (0), PON (1), PAL (2), HYL (3), VAS (4), PPA (5).</param>
        /// <returns>The integer value corresponding to the voting type.</returns>
        /// <exception cref="NotSupportedException">Thrown when the voting type is not recognized.</exception>
        int MapToVotingType(string? voteType);
    }

    /// <summary>
    /// Maps Ahjo voting type codes to integer storage values using case-insensitive matching.
    /// Supports NORMAL/NORMAALI, PON, PAL, HYL, VAS, and PPA voting types.
    /// </summary>
    public class VotingTypeMapper : IVotingTypeMapper
    {
        private readonly Dictionary<string, int> _map = new Dictionary<string, int>()
        {
            { "NORMAL", 0 },
            { "NORMAALI", 0 },
            { "PON", 1 },
            { "PAL", 2 },
            { "HYL", 3 },
            { "VAS", 4 },
            { "PPA", 5 },
        };

        public int MapToVotingType(string? voteType)
        {
            if (voteType == null || !_map.ContainsKey(voteType.ToUpper()))
            {
                throw new NotSupportedException("Unknown voting type: " + voteType);
            }

            return _map[voteType.ToUpper()];
        }
    }
}
