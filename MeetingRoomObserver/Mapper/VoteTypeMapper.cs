using AutoMapper;
using MeetingRoomObserver.Handler.DTOs;
using MeetingRoomObserver.Models;
using MeetingRoomObserver.StorageClient;

namespace MeetingRoomObserver.Mapper
{
    public interface IVoteTypeMapper
    {
        int MapToVoteType(string? voteType);
    }

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
