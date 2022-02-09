namespace Signia.Event;

public interface IEventHandler
{
    public Type EventType { get; }
    Task Handle(IEvent evt);
}