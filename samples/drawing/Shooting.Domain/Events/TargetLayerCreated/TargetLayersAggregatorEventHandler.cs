using MediatR;
using Microsoft.Extensions.Logging;
using Shooting.Domain.Infrastructure;
using Shooting.Domain.IntegrationEvents.TargetLayersCreated;

namespace Shooting.Domain.Events.TargetLayerCreated;

public class TargetLayersAggregatorEventHandler : INotificationHandler<TargetLayerCreatedEvent>
{
    private readonly TargetLayersStorage _targetLayersStorage;
    private readonly IMediator _mediator;
    private readonly ILogger<TargetLayersAggregatorEventHandler> _logger;

    public TargetLayersAggregatorEventHandler(TargetLayersStorage targetLayersStorage, ILogger<TargetLayersAggregatorEventHandler> logger, IMediator mediator)
    {
        _targetLayersStorage = targetLayersStorage;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(TargetLayerCreatedEvent notification, CancellationToken cancellationToken)
    {
        _targetLayersStorage.AddOrUpdate(notification.Lane, notification.TargetLayer);
        _logger.LogInformation("Updated {layer} storage for Lane {laneNumber}.", notification.TargetLayer.Layer, notification.Lane);
        var layers = _targetLayersStorage.GetTargetLayers(notification.Lane);
        await _mediator.Publish(new TargetLayersCreatedIntegrationEvent(layers.Select(l => l.Layer).ToList()), cancellationToken);
    }
}