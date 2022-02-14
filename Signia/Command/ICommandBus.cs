using Signia.Application;

namespace Signia.Command;

public interface ICommandBus : IBus<ICommandHandler>
{
    public Action<ICommand>? BeforeExecute { get; set; }
    public Action<ICommand>? AfterExecute { get; set; }

    Task Execute(ICommand command);
}