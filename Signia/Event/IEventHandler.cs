using Signia.Application;

namespace Signia.Event;

public interface IEventHandler : IHandler
{
    public Type EventType { get; }
    Task Handle(IEvent evt);
}