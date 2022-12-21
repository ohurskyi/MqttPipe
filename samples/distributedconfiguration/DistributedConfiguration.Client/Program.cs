using DistributedConfiguration.Client;
using MqttPipe.Infrastructure;
using MqttPipe.Configuration.DependencyInjection;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, serviceCollection) =>
    {
        serviceCollection.AddInfrastructureMqttPipe(hostContext.Configuration);

        serviceCollection.AddDomainServices();

        serviceCollection.AddHostedService<BackgroundPublisher>();

        serviceCollection.AddHostedService<BackgroundRequestSender>();
    })
    .UseSerilog((hostingContext, _, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console())
    .Build();

host.Services.UseMqttMessageReceivedHandler<InfrastructureClientOptions>();

await host.RunAsync();