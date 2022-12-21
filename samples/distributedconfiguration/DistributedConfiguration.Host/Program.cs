using DistributedConfiguration.Domain;
using MqttPipe.Infrastructure;
using MqttPipe.Configuration.DependencyInjection;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, serviceCollection) =>
    {
        var configuration = hostContext.Configuration;
        
        serviceCollection.AddInfrastructureMqttPipe(configuration);

        serviceCollection.AddPairingDomainServices();
    })
    .UseSerilog((hostingContext, _, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console())
    .Build();

host.Services.UseMqttMessageReceivedHandler<InfrastructureClientOptions>();

await host.RunAsync();