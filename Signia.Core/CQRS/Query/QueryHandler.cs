namespace Signia.Core.CQRS.Query;

public abstract class QueryHandler<TQuery, TOutput> : IQueryHandler where TQuery : IQuery<TOutput>
{
    public Type QueryType => typeof(TQuery);

    protected abstract Task<TOutput> Handle(TQuery query);

    public Task<T> QueryAsync<T>(IQuery<T> query)
    {
        if (query is not TQuery typedQuery)
        {
            throw new Exception(
                $"QueryHandlerType=[{GetType().Name}] " +
                $"expected QueryType=[{typeof(TQuery).Name}] " +
                $"but received QueryType=[{query.GetType().Name}]"
            );
        }

        var task = Handle(typedQuery);
        if (task is not Task<T> convertedTask)
        {
            throw new Exception(
                $"QueryHandlerType=[{GetType().Name}] " +
                $"expected return value of Type=[{typeof(T).Name}] " +
                $"but received return value of Type=[{typeof(TOutput).Name}]"
            );
        }

        return convertedTask;
    }

    public T Query<T>(IQuery<T> query)
    {
        var task = QueryAsync(query);
        task.Wait();
        return task.Result;
    }
}