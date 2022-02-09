using Signia.Application;

namespace Signia.Command;

public interface ICommandHandler : IHandler
{
    public Type CommandType { get; }
    Task Execute(ICommand command);
}