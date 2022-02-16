using Signia.Core.CQRS.Application;

namespace Signia.Core.CQRS.Event;

public interface IEventBus : IBus<IEventHandler>
{
    void Publish(IEvent evt);
    Task PublishAsync(IEvent evt);
}