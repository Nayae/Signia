using Signia.Core.CQRS.Command;
using Signia.Core.CQRS.Event;

namespace Signia.Core.CQRS.Saga;

public abstract class SagaHandler : ISagaHandler
{
    public List<Type> EventTypes { get; }

    protected abstract ISagaDescriptor[] ConfigureSagas();

    private readonly Dictionary<Type, ISagaDescriptor> _descriptors;

    protected SagaHandler()
    {
        _descriptors = new Dictionary<Type, ISagaDescriptor>();

        EventTypes = new List<Type>();

        MapDescriptors();
    }

    public Task<ICommand>? Handle(IEvent evt)
    {
        if (!_descriptors.TryGetValue(evt.GetType(), out var descriptor))
        {
            throw new Exception($"Expected SagaHandler for EventType=[{evt.GetType().Name}] but none could be found");
        }

        return descriptor.Handle(evt);
    }

    private void MapDescriptors()
    {
        foreach (var descriptor in ConfigureSagas())
        {
            if (_descriptors.ContainsKey(descriptor.EventType))
            {
                throw new Exception(
                    $"Cannot register duplicate sagas for EventType=[{descriptor.EventType.Name}] " +
                    $"in SagaHandlerType=[{GetType().Name}]"
                );
            }

            _descriptors.Add(descriptor.EventType, descriptor);
            EventTypes.Add(descriptor.EventType);
        }
    }
}