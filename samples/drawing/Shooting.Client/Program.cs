using MessagingLibrary.Core.Configuration.DependencyInjection;
using MessagingLibrary.Processing.Configuration.DependencyInjection;
using MqttPipe.Configuration.DependencyInjection;
using MqttPipe.Shooting;
using Serilog;
using Shooting.Client;
using Shooting.Client.Consumers;
using Shooting.Client.Handlers.TargetLayersCreated;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddShootingMqttPipe(hostContext.Configuration);
        services.AddMessageHandlers(typeof(TargetLayerCreatedMessageHandler).Assembly);
        services.AddConsumerDefinitionListenerProvider<ConsumerDefinitionListenerProvider>();
        services.AddMessageConsumersHostedService();

        services.AddHostedService<BackgroundPublisher>();
        //services.AddHostedService<BackgroundRequestSender>();
    })
    .UseSerilog((hostingContext, _, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console())
    .Build();

host.Services.UseMqttMessageReceivedHandler<ShootingClientOptions>();

await host.RunAsync();