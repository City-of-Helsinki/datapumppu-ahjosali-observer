using MeetingRoomObserver.Handler;
using MeetingRoomObserver.Mapper;
using MeetingRoomObserver.StorageClient;

namespace MeetingRoomObserver
{
    /// <summary>
    /// Coordinates the full event processing pipeline: parsing, mapping, and storage.
    /// </summary>
    public interface IMeetingMessageHandler
    {
        /// <summary>
        /// Processes a raw JSON event message through the full pipeline:
        /// parsing, DTO mapping, and forwarding each resulting event to storage.
        /// </summary>
        /// <param name="jsonBody">The raw JSON body from the Ahjo system or HTTP endpoint.</param>
        Task HandleMessage(string jsonBody);
    }

    /// <summary>
    /// Coordinates the full event processing pipeline by parsing raw JSON messages,
    /// mapping them to storage DTOs, and forwarding each event to the storage layer.
    /// </summary>
    public class MeetingMessageHandler : IMeetingMessageHandler
    {
        private readonly ILogger<MeetingMessageHandler> _logger;
        private readonly IMeetingEventParser _meetingEventParser;
        private readonly IStorageDTOMapper _storageDTOMapper;
        private readonly IStorage _storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeetingMessageHandler"/> class.
        /// </summary>
        /// <param name="meetingEventParser">The parser that deserializes raw JSON into event lists.</param>
        /// <param name="inputDtoToOutputDTOMapper">The mapper that transforms input DTOs to storage DTOs.</param>
        /// <param name="storage">The storage layer that persists events.</param>
        /// <param name="logger">The logger instance.</param>
        public MeetingMessageHandler(
            IMeetingEventParser meetingEventParser,
            IStorageDTOMapper inputDtoToOutputDTOMapper,
            IStorage storage,
            ILogger<MeetingMessageHandler> logger)
        {
            _meetingEventParser = meetingEventParser;
            _storageDTOMapper = inputDtoToOutputDTOMapper;
            _storage = storage;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task HandleMessage(string jsonBody)
        {
            var meetingEvents = _meetingEventParser.ParseJsonMessage(jsonBody);

            var storageEventList = await _storageDTOMapper.MapToStorageDTOs(meetingEvents);
            foreach (var storageEvent in storageEventList)
            {
                await _storage.Add(storageEvent);
            }
        }
    }
}
