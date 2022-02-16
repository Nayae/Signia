using Serilog;
using Signia.Core.CQRS.Command;
using Signia.Core.CQRS.Event;
using Signia.Playground.Domain.Command;
using Signia.Playground.Domain.Event;

namespace Signia.Playground.Application.Command;

public class UpdateJobDescriptionHandler : CommandHandler<UpdateJobDescriptionCommand, JobDescriptionUpdatedEvent>
{
    public UpdateJobDescriptionHandler(ILogger logger, IEventBus eventBus) : base(logger, eventBus)
    {
    }

    protected override Task<JobDescriptionUpdatedEvent>? Handle(UpdateJobDescriptionCommand command)
    {
        return null;
    }
}