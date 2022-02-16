using Serilog;
using Signia.Core.CQRS.Query;
using Signia.Core.CQRS.Saga;
using Signia.Playground.Domain.Command;
using Signia.Playground.Domain.Event;
using Signia.Playground.Domain.Query;

namespace Signia.Playground.Application.Saga;

public class UpdateJobDescriptionSaga : SagaHandler
{
    private readonly ILogger _logger;
    private readonly IQueryBus _queryBus;

    public UpdateJobDescriptionSaga(ILogger logger, IQueryBus queryBus)
    {
        _logger = logger;
        _queryBus = queryBus;
    }

    protected override ISagaDescriptor[] ConfigureSagas() => new ISagaDescriptor[]
    {
        new SagaDescriptor<AssetAddedToJobEvent, UpdateJobDescriptionCommand>(HandleAssetAddedToJobEvent)
    };

    private async Task<UpdateJobDescriptionCommand> HandleAssetAddedToJobEvent(AssetAddedToJobEvent evt)
    {
        var values = await _queryBus.QueryAsync(new GetAssetsByTaskIdQuery());
        _logger.Information("Received Count=[{A}] values from query", values.Length);

        return new UpdateJobDescriptionCommand();
    }
}