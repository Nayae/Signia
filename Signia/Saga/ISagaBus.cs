using Signia.Application;
using Signia.Event;

namespace Signia.Saga;

public interface ISagaBus : IBus<ISagaHandler>
{
    Task Handle(IEvent evt);
}