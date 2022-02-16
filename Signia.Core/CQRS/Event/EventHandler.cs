namespace Signia.Core.CQRS.Event;

public abstract class EventHandler<TEvent> : IEventHandler where TEvent : IEvent
{
    public Type EventType => typeof(TEvent);

    protected abstract Task Handle(TEvent evt);

    public async Task HandleAsync(IEvent evt)
    {
        if (evt is not TEvent typedEvent)
        {
            throw new Exception(
                $"EventHandlerType=[{GetType().Name}] " +
                $"expected EventType=[{typeof(TEvent).Name}] " +
                $"but received EventType=[{evt.GetType().Name}]"
            );
        }

        await Handle(typedEvent);
    }

    public void Handle(IEvent evt)
    {
        HandleAsync(evt).Wait();
    }
}