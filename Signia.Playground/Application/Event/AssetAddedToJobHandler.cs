using Signia.Playground.Domain.Event;

namespace Signia.Playground.Application.Event;

public class AssetAddedToJobHandler : Core.CQRS.Event.EventHandler<AssetAddedToJobEvent>
{
    protected override Task Handle(AssetAddedToJobEvent evt)
    {
        return Task.CompletedTask;
    }
}