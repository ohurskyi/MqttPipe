using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MqttPipe.Configuration.DependencyInjection;

namespace MqttPipe.Shooting;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShootingMqttPipe(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection.AddMqttPipe<ShootingClientOptions, ShootingClientOptionsBuilder>(configuration);
    }
}