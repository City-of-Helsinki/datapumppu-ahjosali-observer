using AutoMapper;
using MeetingRoomObserver.Handler.DTOs;
using MeetingRoomObserver.Models;
using MeetingRoomObserver.StorageClient;

namespace MeetingRoomObserver.Mapper
{
    /// <summary>
    /// Maps Ahjo vote type strings to integer storage values.
    /// </summary>
    public interface IVoteTypeMapper
    {
        /// <summary>
        /// Maps a Finnish vote type string to its integer storage value.
        /// </summary>
        /// <param name="voteType">The vote type: "JAA" (aye), "EI" (nay), "TYHJA" (empty), or "POISSA" (absent).</param>
        /// <returns>0 for aye, 1 for nay, 2 for empty, 3 for absent.</returns>
        /// <exception cref="NotSupportedException">Thrown when the vote type is not recognized.</exception>
        int MapToVoteType(string? voteType);
    }

    /// <summary>
    /// Maps Finnish vote type strings (JAA, EI, TYHJA, POISSA) to integer storage values.
    /// Throws <see cref="NotSupportedException"/> for unknown types.
    /// </summary>
    public class VoteTypeMapper : IVoteTypeMapper
    {
        private readonly Dictionary<string, int> _map = new Dictionary<string, int>()
        {
            { "JAA", 0 },
            { "EI", 1 },
            { "TYHJA", 2 },
            { "POISSA", 3 },
        };

        public int MapToVoteType(string? voteType)
        {
            if (voteType == null || !_map.ContainsKey(voteType))
            {
                throw new NotSupportedException("Unknown vote type: " + voteType);
            }

            return _map[voteType];
        }
    }
}
