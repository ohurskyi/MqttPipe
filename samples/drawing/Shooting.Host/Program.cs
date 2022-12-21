using MqttPipe.Configuration.DependencyInjection;
using MqttPipe.Shooting;
using Serilog;
using Shooting.Domain;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext,services) =>
    {
        services.AddShootingMqttPipe(hostContext.Configuration);
        services.AddShootingDomainServices();
    })
    .UseSerilog((hostingContext, _, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration))
    .Build();

host.Services.UseMqttMessageReceivedHandler<ShootingClientOptions>();

await host.RunAsync();