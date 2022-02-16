using Serilog;
using Signia.Core.CQRS.Event;

namespace Signia.Core.CQRS.Command;

public abstract class CommandHandler<TCommand, TEvent> : ICommandHandler
    where TCommand : ICommand
    where TEvent : IEvent
{
    public Type CommandType => typeof(TCommand);

    protected abstract Task<TEvent>? Handle(TCommand command);

    private readonly ILogger _logger;
    private readonly IEventBus _eventBus;

    protected CommandHandler(ILogger logger, IEventBus eventBus)
    {
        _logger = logger;
        _eventBus = eventBus;
    }

    public async Task ExecuteAsync(ICommand command)
    {
        if (command is not TCommand typedCommand)
        {
            throw new Exception(
                $"CommandHandlerType=[{GetType().Name}] " +
                $"expected CommandType=[{typeof(TCommand).Name}] " +
                $"but received CommandType=[{command.GetType().Name}]"
            );
        }

        var handlingTask = Handle(typedCommand);
        if (handlingTask == null)
        {
            _logger.Verbose("Execution of CommandType=[{A}] led to no state change", typeof(TCommand).Name);
            return;
        }

        await handlingTask;
        await _eventBus.PublishAsync(handlingTask.Result);
    }

    public void Execute(ICommand command)
    {
        ExecuteAsync(command).Wait();
    }
}