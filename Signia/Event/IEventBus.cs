using Signia.Application;

namespace Signia.Event;

public interface IEventBus : IBus<IEventHandler>
{
    public Action<IEvent>? BeforeHandle { get; set; }
    public Action<IEvent>? AfterHandle { get; set; }

    Task Publish(IEvent evt);
}