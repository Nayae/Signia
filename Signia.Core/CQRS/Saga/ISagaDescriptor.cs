using Signia.Core.CQRS.Command;
using Signia.Core.CQRS.Event;

namespace Signia.Core.CQRS.Saga;

public interface ISagaDescriptor
{
    public Type EventType { get; }

    Task<ICommand>? Handle(IEvent evt);
}