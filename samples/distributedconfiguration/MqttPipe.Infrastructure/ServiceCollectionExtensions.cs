using MqttPipe.Configuration.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MqttPipe.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureMqttPipe(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        return serviceCollection.AddMqttPipe<InfrastructureClientOptions, InfrastructureClientOptionsBuilder>(configuration);
    }
}