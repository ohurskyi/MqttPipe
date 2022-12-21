using MediatR;
using MessagingLibrary.Core.Clients;
using Microsoft.Extensions.Logging;
using MqttPipe.Shooting;
using Shooting.Contracts.TargetLayers;
using Shooting.Contracts.Topics;

namespace Shooting.Domain.IntegrationEvents.TargetLayersCreated;

public class TargetLayersCreatedEventHandler : INotificationHandler<TargetLayersCreatedIntegrationEvent>
{
    private readonly IMessageBus<ShootingClientOptions> _messageBus;
    private readonly ILogger<TargetLayersCreatedEventHandler> _logger;

    public TargetLayersCreatedEventHandler(IMessageBus<ShootingClientOptions> messageBus, ILogger<TargetLayersCreatedEventHandler> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task Handle(TargetLayersCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var targetLayerCreatedContract = new TargetLayersCreatedContract(notification.Layers);
        _logger.LogInformation("Received {event}. Publish into message queue to notify other services.", nameof(TargetLayersCreatedIntegrationEvent));
        await _messageBus.Publish(targetLayerCreatedContract, ShootingTopicConstants.TargetCreatedTopic);
    }
}