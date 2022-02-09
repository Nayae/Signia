namespace Signia.Command;

public interface ICommandHandler
{
    public Type CommandType { get; }
    Task Execute(ICommand command);
}