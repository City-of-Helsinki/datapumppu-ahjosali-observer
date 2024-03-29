using MeetingRoomObserver.Handler;
using MeetingRoomObserver.Mapper;
using MeetingRoomObserver.StorageClient;
using MeetingRoomObserver.Events;
using MeetingRoomObserver.Events.Providers;
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

            builder.Services.AddHealthChecks();

            AddDependencyInjections(builder.Services);

            builder.Services.AddLogging(options =>
            {
                options.AddSimpleConsole(c =>
                {
                    c.IncludeScopes = true;
                    c.SingleLine = true;
                    c.TimestampFormat = "dd.MM.yyyy HH:mm:ss ";
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseRouting();
            
            app.UseAuthorization();
            
            app.MapPost("/observer", async (context) =>
            {
                var reader = new StreamReader(context.Request.Body);
                var data = await reader.ReadToEndAsync();
                reader.Close();

                var eventHandler = app.Services.GetService<IMeetingMessageHandler>();
                await eventHandler!.HandleMessage(data);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthz");
                endpoints.MapHealthChecks("/readiness");
            });

            app.Run();
        }

        private static void AddDependencyInjections(IServiceCollection servicess)
        {
            servicess.AddSingleton<IKafkaClientFactory, KafkaClientFactory>();
            servicess.AddHostedService<AhjoSaliEventObserver>();

            servicess.AddTransient<IMeetingEventParser, MeetingEventParser>();
            servicess.AddTransient<IMeetingMessageHandler, MeetingMessageHandler>();

            servicess.AddTransient<IMeetingEventTypeMapper, MeetingEventTypeMapper>();
            servicess.AddTransient<IVoteTypeMapper, VoteTypeMapper>();
            servicess.AddTransient<IVotingTypeMapper, VotingTypeMapper>();
            servicess.AddTransient<ISpeechTypeMapper, SpeechTypeMapper>();
            
            servicess.AddTransient<IStorageDTOMapper, StorageDTOMapper>();
            servicess.AddTransient<IStorage, Storage>();

            servicess.AddTransient<IStorageConnection, StorageConnection>();
            servicess.AddTransient<IStorageApiClient, StorageApiClient>();

            servicess.AddTransient<IStorageKafkaClient, StorageKafkaClient>();

        }
    }
}