using MeetingRoomObserver.StorageClient.DTOs;

namespace MeetingRoomObserver.StorageClient
{
    public interface IStorage
    {
        Task Add(StorageEventDTO storageEventDTO);
    }

    public class Storage : IStorage
    {
        private readonly IStorageKafkaClient _busClient;

        public Storage(IStorageKafkaClient busClient)
        {
            _busClient = busClient;
        }

        public Task Add(StorageEventDTO storageEventDTO)
        {
            return _busClient.SendEvent(storageEventDTO);
        }
    }
}
