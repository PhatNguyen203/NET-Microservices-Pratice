using CommandService.Models;
using System.Collections.Generic;

namespace CommandService.Data
{
    public interface ICommandServiceRepo
    {
        //platforms services
        bool PlatformExists(int id);
        bool ExternalPlatformExists(int externalPlatformId);
        IEnumerable<Platform> GetAllPlatforms();
        void CreatPlatform(Platform platform);

        //commands services
        Command GetCommand(int commandId, int platformId);
        void CreateCommand(int platformId, Command command);
        IEnumerable<Command> GetCommandsForPlatform(int id);   
        
        bool SaveChanges();

    }
}
