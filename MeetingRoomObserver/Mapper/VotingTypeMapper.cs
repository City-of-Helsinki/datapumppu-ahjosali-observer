using AutoMapper;
using MeetingRoomObserver.Handler.DTOs;
using MeetingRoomObserver.Models;
using MeetingRoomObserver.StorageClient;

namespace MeetingRoomObserver.Mapper
{
    public interface IVotingTypeMapper
    {
        int MapToVotingType(string? voteType);
    }

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
