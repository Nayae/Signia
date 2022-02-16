using Signia.Core.CQRS.Application;

namespace Signia.Core.CQRS.Query;

public interface IQueryHandler : IHandler
{
    public Type QueryType { get; }

    T Query<T>(IQuery<T> query);
    Task<T> QueryAsync<T>(IQuery<T> query);
}