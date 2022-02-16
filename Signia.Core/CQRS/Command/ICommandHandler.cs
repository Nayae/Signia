using Signia.Core.CQRS.Application;

namespace Signia.Core.CQRS.Command;

public interface ICommandHandler : IHandler
{
    public Type CommandType { get; }
    
    void Execute(ICommand command);
    Task ExecuteAsync(ICommand command);
}