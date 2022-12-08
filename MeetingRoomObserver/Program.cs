using MeetingRoomObserver.Handler;
using MeetingRoomObserver.Mapper;
using MeetingRoomObserver.StorageClient;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MeetingRoomObserverUnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace MeetingRoomObserver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            AddDependencyInjections(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();
            
            app.MapPost("/observer", async (context) =>
            {
                var reader = new StreamReader(context.Request.Body);
                var data = await reader.ReadToEndAsync();
                reader.Close();

                var eventHandler = app.Services.GetService<IMeetingMessageHandler>();
                await eventHandler!.HandleMessage(data);
            });

            app.Run();
        }

        private static void AddDependencyInjections(IServiceCollection servicess)
        {
            servicess.AddTransient<IMeetingEventParser, MeetingEventParser>();
            servicess.AddTransient<IMeetingMessageHandler, MeetingMessageHandler>();

            servicess.AddTransient<IMeetingEventTypeMapper, MeetingEventTypeMapper>();
            servicess.AddTransient<IVoteTypeMapper, VoteTypeMapper>();
            servicess.AddTransient<IVotingTypeMapper, VotingTypeMapper>();
            

            servicess.AddTransient<IStorageDTOMapper, StorageDTOMapper>();
            servicess.AddTransient<IStorage, Storage>();

            servicess.AddTransient<IStorageServiceBusClient, StorageServiceBusClient>();
            
        }
    }
}