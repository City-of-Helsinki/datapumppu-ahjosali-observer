using MeetingRoomObserver.StorageClient.DTOs;

namespace MeetingRoomObserver.StorageClient
{
    public interface IStorage
    {
        Task Add(StorageEventDTO storageEventDTO);
    }

    public class Storage : IStorage
    {
        private readonly IStorageKafkaClient _kafkaClient;

        public Storage(IStorageKafkaClient kafkaClient)
        {
            _kafkaClient = kafkaClient;
        }

        public Task Add(StorageEventDTO storageEventDTO)
        {
            return _kafkaClient.SendEvent(storageEventDTO);
        }
    }
}
