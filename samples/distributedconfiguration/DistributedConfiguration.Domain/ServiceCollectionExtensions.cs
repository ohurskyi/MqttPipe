using DistributedConfiguration.Domain.Consumers;
using DistributedConfiguration.Domain.Handlers;
using DistributedConfiguration.Domain.Infrastructure;
using MessagingLibrary.Core.Configuration.DependencyInjection;
using MessagingLibrary.Processing.Configuration.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedConfiguration.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPairingDomainServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<DevicesStorage>();
        serviceCollection.AddMessageHandlers(typeof(PairDeviceMessageHandler).Assembly);
        serviceCollection.AddConsumerDefinitionListenerProvider<ConsumerDefinitionListenerProvider>();
        serviceCollection.AddMessageConsumersHostedService();
        return serviceCollection;
    }
}