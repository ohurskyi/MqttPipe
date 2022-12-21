using MediatR;
using MessagingLibrary.Core.Configuration.DependencyInjection;
using MessagingLibrary.Processing.Configuration.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Shooting.Domain.Consumers;
using Shooting.Domain.Events;
using Shooting.Domain.Events.ShootingStartedEvent;
using Shooting.Domain.Events.TargetLayerCreated;
using Shooting.Domain.Handlers.ShootingInfo;
using Shooting.Domain.Infrastructure;

namespace Shooting.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShootingDomainServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMessageHandlers(typeof(CreateTargetLayerMessageHandler).Assembly);
        serviceCollection.AddConsumerDefinitionListenerProvider<ConsumerDefinitionListenerProvider>();
        serviceCollection.AddMessageConsumersHostedService();

        serviceCollection.AddMediatR(typeof(IncrementActivateLanesCountEventHandler));

        serviceCollection.AddDomainInfrastructure();
            
        return serviceCollection;
    }

    private static IServiceCollection AddDomainInfrastructure(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<TargetLayersStorage>()
            .AddSingleton<ActiveShootingLanesStorage>();
    }
}