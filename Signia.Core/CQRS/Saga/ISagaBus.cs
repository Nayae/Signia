using Signia.Core.CQRS.Application;
using Signia.Core.CQRS.Event;

namespace Signia.Core.CQRS.Saga;

public interface ISagaBus : IBus<ISagaHandler>
{
    void Handle(IEvent evt);
    Task HandleAsync(IEvent evt);
}