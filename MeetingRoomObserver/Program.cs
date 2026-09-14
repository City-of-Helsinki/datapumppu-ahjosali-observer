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
    /// <summary>
    /// Application entry point. Configures the ASP.NET Core host, dependency injection,
    /// HTTP pipeline, health checks, and the <c>POST /observer</c> endpoint.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Builds and runs the ASP.NET Core web application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            app.MapPost("/observer", async (HttpContext context, IMeetingMessageHandler eventHandler, IConfiguration configuration) =>
            {
                var apiKey = configuration["OBSERVER_API_KEY"];
                if (!string.IsNullOrEmpty(apiKey) && context.Request.Headers["X-API-KEY"] != apiKey)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                var reader = new StreamReader(context.Request.Body);
                var data = await reader.ReadToEndAsync();
                await eventHandler.HandleMessage(data);
            });

            app.MapHealthChecks("/healthz");
            app.MapHealthChecks("/readiness");

            app.Run();
        }

        private static void AddDependencyInjections(IServiceCollection services)
        {
            services.AddSingleton<IKafkaClientFactory, KafkaClientFactory>();
            services.AddHostedService<AhjoSaliEventObserver>();

            services.AddSingleton<IMeetingEventParser, MeetingEventParser>();
            services.AddSingleton<IMeetingMessageHandler, MeetingMessageHandler>();

            services.AddSingleton<IMeetingEventTypeMapper, MeetingEventTypeMapper>();
            services.AddSingleton<IVoteTypeMapper, VoteTypeMapper>();
            services.AddSingleton<IVotingTypeMapper, VotingTypeMapper>();
            services.AddSingleton<ISpeechTypeMapper, SpeechTypeMapper>();
            
            services.AddSingleton<IStorageDTOMapper, StorageDTOMapper>();
            services.AddSingleton<IStorage, Storage>();

            services.AddSingleton<IStorageConnection, StorageConnection>();
            services.AddSingleton<IStorageApiClient, StorageApiClient>();

            services.AddSingleton<IStorageKafkaClient, StorageKafkaClient>();
        }
    }
}
