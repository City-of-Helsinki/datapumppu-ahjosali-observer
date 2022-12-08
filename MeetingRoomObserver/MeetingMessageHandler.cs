using MeetingRoomObserver.Handler;
using MeetingRoomObserver.Mapper;
using MeetingRoomObserver.StorageClient;

namespace MeetingRoomObserver
{
    public interface IMeetingMessageHandler
    {
        Task HandleMessage(string jsonBody);
    }

    public class MeetingMessageHandler : IMeetingMessageHandler
    {
        private readonly IMeetingEventParser _meetingEventParser;
        private readonly IStorageDTOMapper _storageDTOMapper;
        private readonly IStorage _storage;

        public MeetingMessageHandler(
            IMeetingEventParser meetingEventParser,
            IStorageDTOMapper inputDtoToOutputDTOMapper,
            IStorage storage)
        {
            _meetingEventParser = meetingEventParser;
            _storageDTOMapper = inputDtoToOutputDTOMapper;
            _storage = storage;
        }

        public async Task HandleMessage(string jsonBody)
        {
            var meetingEvents = _meetingEventParser.ParseJsonMessage(jsonBody);

            var storageEventList = _storageDTOMapper.MapToStorageDTOs(meetingEvents);
            foreach (var storageEvent in storageEventList)
            {
                await _storage.Add(storageEvent);
            }
        }
    }
}
