﻿using Signia.Command;
using Signia.Event;

namespace Signia.Saga;

public class SagaDescriptor<TEvent, TCommand> : ISagaDescriptor
    where TEvent : IEvent
    where TCommand : ICommand
{
    public delegate Task<TCommand> SagaDelegate(TEvent evt);

    public Type EventType => typeof(TEvent);

    private readonly SagaDelegate _handler;

    public SagaDescriptor(SagaDelegate handler)
    {
        _handler = handler;
    }

    public async Task<ICommand> Handle(IEvent evt)
    {
        return await _handler((TEvent)evt);
    }
}