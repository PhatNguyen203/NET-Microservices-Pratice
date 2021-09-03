using System.Threading.Tasks;
using PlatformService.DTOs;

namespace PlatformService.SyncDataServices.Http
{
    public interface ICommandDataClients
    {
         Task SendNewPlatformToCommand(PlatformReadDTO platformDTO);
    }
}