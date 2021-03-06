using Serilog;
using Signia.Core.CQRS.Saga;

namespace Signia.Core.CQRS.Event;

public class EventBus : IEventBus
{
    private readonly ILogger _logger;
    private readonly ISagaBus _sagaBus;
    private readonly Dictionary<Type, IEventHandler> _handlers;

    public EventBus(ILogger logger, ISagaBus sagaBus)
    {
        _logger = logger;
        _sagaBus = sagaBus;
        _handlers = new Dictionary<Type, IEventHandler>();
    }

    public void MapHandlers(IEnumerable<IEventHandler> handlers)
    {
        foreach (var handler in handlers)
        {
            _handlers.Add(handler.EventType, handler);
        }
    }

    public async Task PublishAsync(IEvent evt)
    {
        if (!_handlers.TryGetValue(evt.GetType(), out var handler))
        {
            _logger.Verbose("No EventHandler registered for EventType=[{A}]", evt.GetType().Name);
            return;
        }

        _logger.Verbose("Started handling of EventType=[{A}]", evt.GetType().Name);

        await Task.WhenAll(
            handler.HandleAsync(evt),
            _sagaBus.HandleAsync(evt)
        );

        _logger.Verbose("Finished handling of EventType=[{A}]", evt.GetType().Name);
    }

    public void Publish(IEvent evt)
    {
        PublishAsync(evt).Wait();
    }
}