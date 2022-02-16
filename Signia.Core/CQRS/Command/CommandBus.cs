using Serilog;

namespace Signia.Core.CQRS.Command;

public class CommandBus : ICommandBus
{
    private readonly ILogger _logger;
    private readonly Dictionary<Type, ICommandHandler> _handlers;

    public CommandBus(ILogger logger)
    {
        _logger = logger;
        _handlers = new Dictionary<Type, ICommandHandler>();
    }

    public void MapHandlers(IEnumerable<ICommandHandler> handlers)
    {
        foreach (var handler in handlers)
        {
            _handlers.Add(handler.CommandType, handler);
        }
    }

    public async Task ExecuteAsync(ICommand command)
    {
        if (!_handlers.TryGetValue(command.GetType(), out var handler))
        {
            _logger.Error("No CommandHandler registered for CommandType=[{A}]", command.GetType().Name);
            return;
        }

        _logger.Verbose("Started execution of CommandType=[{A}]", command.GetType().Name);

        await handler.ExecuteAsync(command);

        _logger.Verbose("Finished execution of CommandType=[{A}]", command.GetType().Name);
    }

    public void Execute(ICommand command)
    {
        ExecuteAsync(command).Wait();
    }
}