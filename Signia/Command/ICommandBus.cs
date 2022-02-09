using Signia.Application;

namespace Signia.Command;

public interface ICommandBus : IBus<ICommandHandler>
{
    Task Execute(ICommand command);
}