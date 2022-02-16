using Serilog;

namespace Signia.Core.CQRS.Event;

public abstract class EventHandler<T> : IEventHandler where T : IEvent
{
    public Type EventType => typeof(T);

    protected abstract Task Handle(T evt);

    private readonly ILogger _logger;

    protected EventHandler(ILogger logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(IEvent evt)
    {
        if (evt is not T typedEvent)
        {
            _logger.Error(
                "EventHandlerType=[{A}] expected EventType=[{B}] but received EventType=[{C}]",
                GetType().Name,
                typeof(T).Name,
                evt.GetType().Name
            );
            return;
        }

        await Handle(typedEvent);
    }

    public void Handle(IEvent evt)
    {
        HandleAsync(evt).Wait();
    }
}