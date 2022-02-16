using Signia.Core.CQRS.Application;

namespace Signia.Core.CQRS.Query;

public interface IQueryBus : IBus<IQueryHandler>
{
    T Query<T>(IQuery<T> query);
    Task<T> QueryAsync<T>(IQuery<T> query);
}