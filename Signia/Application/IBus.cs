namespace Signia.Application;

public interface IBus<in T>
{
    void ConfigureHandlers(IEnumerable<T> handlers);
}