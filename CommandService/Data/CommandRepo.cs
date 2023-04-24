using CommandService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandService.Data
{
    public class CommandRepo : ICommandServiceRepo
    {
        private readonly CommandDbContext context;

        public CommandRepo(CommandDbContext context)
        {
            this.context = context;
        }
        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            context.Commands.Add(command);
        }

        public void CreatPlatform(Platform platform)
        {
           if(platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
           context.Platforms.Add(platform);

        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return context.Platforms.ToList();
        }

        public Command GetCommand(int commandId, int platformId)
        {
            return context.Commands.Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int id)
        {
            return context.Commands.Where(c => c.PlatformId == id)
                    .OrderBy(c => c.Platform.Name);
        }

        public bool PlatformExists(int id)
        {
            return context.Platforms.Any(p => p.Id == id);
        }

        public bool ExternalPlatformExists(int externalPlatformId)
        {
            return context.Platforms.Any(p => p.ExternalId == externalPlatformId);
        }

        public bool SaveChanges()
        {
            return (context.SaveChanges() >= 0);
        }
    }
}
