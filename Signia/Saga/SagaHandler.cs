using Serilog;
using Signia.Command;
using Signia.Event;

namespace Signia.Saga;

public abstract class SagaHandler : ISagaHandler
{
    public List<Type> EventTypes { get; }

    protected abstract ISagaDescriptor[] ConfigureSagas();

    private readonly ILogger _logger;
    private readonly Dictionary<Type, ISagaDescriptor> _descriptors;

    protected SagaHandler(ILogger logger)
    {
        _logger = logger;
        _descriptors = new Dictionary<Type, ISagaDescriptor>();

        EventTypes = new List<Type>();

        MapDescriptors();
    }

    public Task<ICommand>? Handle(IEvent evt)
    {
        if (!_descriptors.TryGetValue(evt.GetType(), out var descriptor))
        {
            _logger.Error(
                "Expected SagaHandler for EventType=[{A}] but none could be found",
                evt.GetType().Name
            );
            return null;
        }

        return descriptor.Handle(evt);
    }

    private void MapDescriptors()
    {
        foreach (var descriptor in ConfigureSagas())
        {
            if (_descriptors.ContainsKey(descriptor.EventType))
            {
                _logger.Fatal(
                    "Cannot register duplicate sagas for EventType=[{A}] in SagaHandlerType=[{B}]",
                    descriptor.EventType.Name,
                    GetType().Name
                );
                throw new Exception();
            }

            _descriptors.Add(descriptor.EventType, descriptor);
            EventTypes.Add(descriptor.EventType);
        }
    }
}