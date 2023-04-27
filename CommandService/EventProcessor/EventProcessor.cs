using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace CommandService.EventProcessor
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IMapper mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            this.scopeFactory = scopeFactory;
            this.mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            //check event type
            switch (eventType.Event)
            {
                case "Platform Published":
                    Console.WriteLine("--> Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandServiceRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatfomPublishedDto>(platformPublishedMessage);
                try
                {
                    var platform = mapper.Map<Platform>(platformPublishedDto);
                    if (!repo.ExternalPlatformExists(platform.ExternalId))
                    {
                        repo.CreatPlatform(platform);
                        repo.SaveChanges();
                        Console.WriteLine("--> Platform added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exisits...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
                }
            }
        }

        enum EventType
        {
            PlatformPublished,
            Undetermined
        }
    }
}
