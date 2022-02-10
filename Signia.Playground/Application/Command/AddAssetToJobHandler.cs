using Serilog;
using Signia.Command;
using Signia.Event;
using Signia.Playground.Domain.Command;
using Signia.Playground.Domain.Event;

namespace Signia.Playground.Application.Command;

public class AddAssetToJobHandler : CommandHandler<AddAssetToJobCommand, AssetAddedToJobEvent>
{
    public AddAssetToJobHandler(ILogger logger, IEventBus eventBus) : base(logger, eventBus)
    {
    }

    protected override Task<AssetAddedToJobEvent> Handle(AddAssetToJobCommand command)
    {
        return Task.FromResult(
            new AssetAddedToJobEvent()
        );
    }
}