namespace CommandService.EventProcessor
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
