using Serilog;
using Signia.Playground.Domain.Event;

namespace Signia.Playground.Application.Event;

public class AssetAddedToJobHandler : Signia.Event.EventHandler<AssetAddedToJobEvent>
{
    public AssetAddedToJobHandler(ILogger logger) : base(logger)
    {
    }

    protected override Task Handle(AssetAddedToJobEvent evt)
    {
        return Task.CompletedTask;
    }
}