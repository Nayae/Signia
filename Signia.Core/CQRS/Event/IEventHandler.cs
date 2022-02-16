using Signia.Core.CQRS.Application;

namespace Signia.Core.CQRS.Event;

public interface IEventHandler : IHandler
{
    public Type EventType { get; }

    void Handle(IEvent evt);
    Task HandleAsync(IEvent evt);
}