using Signia.Command;
using Signia.Event;

namespace Signia.Saga;

public interface ISagaHandler
{
    public List<Type> EventTypes { get; }

    Task<ICommand>? Handle(IEvent evt);
}