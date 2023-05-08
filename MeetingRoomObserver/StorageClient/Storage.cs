using MeetingRoomObserver.StorageClient.DTOs;

namespace MeetingRoomObserver.StorageClient
{
    public interface IStorage
    {
        Task Add(StorageEventDTO storageEventDTO);
    }

    public class Storage : IStorage
    {
        private readonly ILogger<Storage> _logger;
        private readonly IStorageKafkaClient _kafkaClient;

        public Storage(ILogger<Storage> logger, IStorageKafkaClient kafkaClient)
        {
            _logger = logger;
            _kafkaClient = kafkaClient;
        }

        public Task Add(StorageEventDTO storageEventDTO)
        {
            if (string.IsNullOrEmpty(storageEventDTO.MeetingID))
            {
                _logger.LogError("unknown meeting id, ignoring event");
                return Task.CompletedTask;
            }
            return _kafkaClient.SendEvent(storageEventDTO);
        }
    }
}
