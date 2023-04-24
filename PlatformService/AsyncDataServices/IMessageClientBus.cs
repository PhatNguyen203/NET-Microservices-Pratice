using PlatformService.Dtos;

namespace PlatformService.AsyncDataServices
{
    public interface IMessageClientBus
    {
        void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
    }
}
