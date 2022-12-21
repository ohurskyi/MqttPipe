using DistributedConfiguration.Client.Consumers;
using DistributedConfiguration.Client.IntegrationEvents.PairedDevicesConfigurationChanged;
using DistributedConfiguration.Contracts.Pairing;
using MessagingLibrary.Core.Configuration.DependencyInjection;
using MessagingLibrary.Processing.Configuration.DependencyInjection;

namespace DistributedConfiguration.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMessageHandler<UpdateLocalConfigurationMessageHandler>();
        serviceCollection.AddMessageHandler<NotifyUsersMessageHandler>();
        serviceCollection.AddConsumerDefinitionListenerProvider<ConsumerDefinitionListenerProvider>();
        serviceCollection.AddMessageConsumersHostedService();
        return serviceCollection;
    }
}