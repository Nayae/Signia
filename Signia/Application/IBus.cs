namespace Signia.Application;

public interface IBus<in T> where T : IHandler
{
    void MapHandlers(IEnumerable<T> handlers);
}