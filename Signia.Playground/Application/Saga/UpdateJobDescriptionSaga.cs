using Serilog;
using Signia.Playground.Domain.Command;
using Signia.Playground.Domain.Event;
using Signia.Saga;

namespace Signia.Playground.Application.Saga;

public class UpdateJobDescriptionSaga : SagaHandler
{
    public UpdateJobDescriptionSaga(ILogger logger) : base(logger)
    {
    }

    protected override ISagaDescriptor[] ConfigureSagas() => new ISagaDescriptor[]
    {
        new SagaDescriptor<AssetAddedToJobEvent, UpdateJobDescriptionCommand>(HandleAssetAddedToJobEvent)
    };

    private Task<UpdateJobDescriptionCommand> HandleAssetAddedToJobEvent(AssetAddedToJobEvent evt)
    {
        return Task.FromResult(
            new UpdateJobDescriptionCommand()
        );
    }
}