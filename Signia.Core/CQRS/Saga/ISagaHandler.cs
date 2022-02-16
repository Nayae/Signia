using Signia.Core.CQRS.Application;
using Signia.Core.CQRS.Command;
using Signia.Core.CQRS.Event;

namespace Signia.Core.CQRS.Saga;

public interface ISagaHandler : IHandler
{
    public List<Type> EventTypes { get; }

    Task<ICommand>? Handle(IEvent evt);
}