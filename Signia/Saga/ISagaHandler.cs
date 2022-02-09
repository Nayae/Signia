using Signia.Application;
using Signia.Command;
using Signia.Event;

namespace Signia.Saga;

public interface ISagaHandler : IHandler
{
    public List<Type> EventTypes { get; }

    Task<ICommand>? Handle(IEvent evt);
}