using Serilog;

namespace Signia.Core.CQRS.Query;

public class QueryBus : IQueryBus
{
    private readonly ILogger _logger;
    private readonly Dictionary<Type, IQueryHandler> _handlers;

    public QueryBus(ILogger logger)
    {
        _logger = logger;
        _handlers = new Dictionary<Type, IQueryHandler>();
    }

    public void MapHandlers(IEnumerable<IQueryHandler> handlers)
    {
        foreach (var handler in handlers)
        {
            _handlers.Add(handler.QueryType, handler);
        }
    }

    public Task<T> QueryAsync<T>(IQuery<T> query)
    {
        if (!_handlers.TryGetValue(query.GetType(), out var handler))
        {
            throw new Exception($"No QueryHandler registered for QueryType=[{query.GetType().Name}]");
        }

        _logger.Verbose("Querying for QueryType=[{A}]", query.GetType().Name);
        return handler.QueryAsync(query);
    }

    public T Query<T>(IQuery<T> query)
    {
        var task = QueryAsync(query);
        task.Wait();
        return task.Result;
    }
}