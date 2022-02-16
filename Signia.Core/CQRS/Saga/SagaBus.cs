using Serilog;
using Signia.Core.CQRS.Command;
using Signia.Core.CQRS.Event;

namespace Signia.Core.CQRS.Saga;

public class SagaBus : ISagaBus
{
    private readonly ILogger _logger;
    private readonly ICommandBus _commandBus;
    private readonly Dictionary<Type, List<ISagaHandler>> _handlers;

    public SagaBus(ILogger logger, ICommandBus commandBus)
    {
        _logger = logger;
        _commandBus = commandBus;

        _handlers = new Dictionary<Type, List<ISagaHandler>>();
    }

    public void MapHandlers(IEnumerable<ISagaHandler> handlers)
    {
        foreach (var handler in handlers)
        {
            foreach (var eventType in handler.EventTypes)
            {
                if (!_handlers.TryGetValue(eventType, out var handlerList))
                {
                    _handlers[eventType] = handlerList = new List<ISagaHandler>();
                }

                handlerList.Add(handler);
            }
        }
    }

    public async Task HandleAsync(IEvent evt)
    {
        if (!_handlers.TryGetValue(evt.GetType(), out var handlers))
        {
            _logger.Verbose("No SagaHandler registered for EventType=[{A}]", evt.GetType().Name);
            return;
        }

        _logger.Verbose("Found {A} saga(s) for EventType=[{B}]", handlers.Count, evt.GetType().Name);
        await Task.WhenAll(
            handlers.Select(saga => HandleSaga(saga, evt)).ToArray()
        );
    }

    public void Handle(IEvent evt)
    {
        HandleAsync(evt).Wait();
    }

    private async Task HandleSaga(ISagaHandler saga, IEvent evt)
    {
        _logger.Verbose(
            "Started handling of SagaType=[{A}] for EventType=[{B}]",
            saga.GetType().Name,
            evt.GetType().Name
        );

        var handlingTask = saga.Handle(evt);

        if (handlingTask == null)
        {
            _logger.Verbose("Execution of SagaType=[{A}] led to no state change", saga.GetType().Name);
            return;
        }

        _logger.Verbose(
            "Execution of SagaType=[{A}] produced a command of CommandType=[{B}]",
            saga.GetType().Name,
            handlingTask.Result.GetType().Name
        );

        await handlingTask;
        await _commandBus.ExecuteAsync(handlingTask.Result);

        _logger.Verbose(
            "Finished handling of SagaType=[{A}] for EventType=[{B}]",
            saga.GetType().Name,
            evt.GetType().Name
        );
    }
}