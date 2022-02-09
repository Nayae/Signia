using Signia.Application;

namespace Signia.Event;

public interface IEventBus : IBus<IEventHandler>
{
    Task Publish(IEvent evt);
}