using Signia.Core.CQRS.Application;

namespace Signia.Core.CQRS.Command;

public interface ICommandBus : IBus<ICommandHandler>
{
    void Execute(ICommand command);
    Task ExecuteAsync(ICommand command);
}