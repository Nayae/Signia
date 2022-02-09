using Signia.Command;
using Signia.Event;

namespace Signia.Saga;

public interface ISagaDescriptor
{
    public Type EventType { get; }

    Task<ICommand>? Handle(IEvent evt);
}