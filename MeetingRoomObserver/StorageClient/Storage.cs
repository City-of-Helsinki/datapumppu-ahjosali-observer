using MeetingRoomObserver.StorageClient.DTOs;

namespace MeetingRoomObserver.StorageClient
{
    public interface IStorage
    {
        Task Add(StorageEventDTO storageEventDTO);
    }

    public class Storage : IStorage
    {
        private readonly IConfiguration _configuration;

        private readonly IStorageServiceBusClient _busClient;

        private readonly IStorageKafkaClient _kafkaClient;

        public Storage(
            IConfiguration configuration,
            IStorageServiceBusClient busClient,
            IStorageKafkaClient kafkaClient)
        {
            _configuration = configuration;
            _busClient = busClient;
            _kafkaClient = kafkaClient;
        }

        public Task Add(StorageEventDTO storageEventDTO)
        {
            if (!string.IsNullOrEmpty(_configuration["SB_CONNECTION_STRING"]))
            {
                return _busClient.SendEvent(storageEventDTO);
            }

            return _kafkaClient.SendEvent(storageEventDTO);
        }
    }
}
