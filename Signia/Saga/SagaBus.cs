﻿using Serilog;
using Signia.Command;
using Signia.Event;

namespace Signia.Saga;

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

    public void ConfigureHandlers(IEnumerable<ISagaHandler> handlers)
    {
        MapSagaHandlers(handlers);
    }

    public async Task Handle(IEvent evt)
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

        await handlingTask;
        await _commandBus.Execute(handlingTask.Result);

        _logger.Verbose(
            "Finished handling of SagaType=[{A}] for EventType=[{B}]",
            saga.GetType().Name,
            evt.GetType().Name
        );
    }

    private void MapSagaHandlers(IEnumerable<ISagaHandler> sagaHandlers)
    {
        foreach (var sagaHandler in sagaHandlers)
        {
            foreach (var eventType in sagaHandler.EventTypes)
            {
                if (!_handlers.TryGetValue(eventType, out var sagas))
                {
                    _handlers[eventType] = sagas = new List<ISagaHandler>();
                }

                sagas.Add(sagaHandler);
            }
        }
    }
}