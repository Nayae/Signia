using Serilog;
using Signia.Event;

namespace Signia.Command;

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

    public async Task Execute(ICommand command)
    {
        if (command is not TCommand typedCommand)
        {
            _logger.Error(
                "CommandHandlerType=[{A}] expected CommandType=[{B}] but received CommandType=[{C}]",
                GetType().Name,
                typeof(TCommand).Name,
                command.GetType().Name
            );
            return;
        }

        var handlingTask = Handle(typedCommand);
        if (handlingTask == null)
        {
            _logger.Verbose("Execution of CommandType=[{A}] led to no state change", typeof(TCommand).Name);
            return;
        }

        await handlingTask;
        await _eventBus.Publish(handlingTask.Result);
    }
}