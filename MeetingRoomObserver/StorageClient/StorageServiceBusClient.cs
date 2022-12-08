using Azure.Messaging.ServiceBus;
using MeetingRoomObserver.StorageClient.DTOs;
using Newtonsoft.Json;

namespace MeetingRoomObserver.StorageClient
{
    public interface IStorageServiceBusClient
    {
        Task SendEvent(StorageEventDTO storageEventDTO);
    }

    public class StorageServiceBusClient : IStorageServiceBusClient
    {
        private readonly IConfiguration _configuration;

        public StorageServiceBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEvent(StorageEventDTO storageEventDTO)
        {
            var sender = CreateServiceBusClient();
            var jsonBody = JsonConvert.SerializeObject(storageEventDTO);            
            return sender.SendMessageAsync(new ServiceBusMessage(jsonBody));
        }

        private ServiceBusSender CreateServiceBusClient()
        {
            var client = new ServiceBusClient(_configuration["ServiceBus:ConnectionString"]);
            return client.CreateSender(_configuration["ServiceBus:QueueName"]);
        }

    }
}
