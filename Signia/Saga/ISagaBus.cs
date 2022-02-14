using Signia.Application;
using Signia.Event;

namespace Signia.Saga;

public interface ISagaBus : IBus<ISagaHandler>
{
    public Action<ISagaHandler, IEvent>? BeforeHandle { get; set; }
    public Action<ISagaHandler, IEvent>? AfterHandle { get; set; }

    Task Handle(IEvent evt);
}