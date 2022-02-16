using Autofac;
using Serilog;
using Signia.Core.CQRS.Application;
using Signia.Core.CQRS.Command;
using Signia.Core.CQRS.Event;
using Signia.Core.CQRS.Saga;
using Signia.Playground.Application.Command;
using Signia.Playground.Application.Event;
using Signia.Playground.Application.Saga;
using Signia.Playground.Domain.Command;

void RegisterBus<TBusInterface, TBusImplementation>(ContainerBuilder builder)
    where TBusImplementation : TBusInterface
    where TBusInterface : notnull
{
    builder.RegisterType<TBusImplementation>()
        .As<TBusInterface>()
        .SingleInstance();
}

void MapHandlers<TBus, THandler>(IContainer container)
    where TBus : IBus<THandler>
    where THandler : IHandler
{
    container.Resolve<TBus>().MapHandlers(
        container.Resolve<IEnumerable<THandler>>()
    );
}

var builder = new ContainerBuilder();

RegisterBus<ICommandBus, CommandBus>(builder);
RegisterBus<IEventBus, EventBus>(builder);
RegisterBus<ISagaBus, SagaBus>(builder);

builder.RegisterType<AddAssetToJobHandler>()
    .As<ICommandHandler>();

builder.RegisterType<UpdateJobDescriptionHandler>()
    .As<ICommandHandler>();

builder.RegisterType<AssetAddedToJobHandler>()
    .As<IEventHandler>();

builder.RegisterType<UpdateJobDescriptionSaga>()
    .As<ISagaHandler>();

builder.RegisterInstance(
    new LoggerConfiguration()
        .WriteTo.Console()
        .MinimumLevel.Verbose()
        .CreateLogger()
).As<ILogger>();

var container = builder.Build();

MapHandlers<ICommandBus, ICommandHandler>(container);
MapHandlers<IEventBus, IEventHandler>(container);
MapHandlers<ISagaBus, ISagaHandler>(container);

var commandBus = container.Resolve<ICommandBus>();
await commandBus.ExecuteAsync(new AddAssetToJobCommand());