namespace Signia.Application;

public interface IBus<in T> where T : IHandler
{
    void ConfigureHandlers(IEnumerable<T> handlers);
}