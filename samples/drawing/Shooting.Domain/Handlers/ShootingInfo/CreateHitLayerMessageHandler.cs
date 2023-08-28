using MediatR;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using Microsoft.Extensions.Logging;
using Shooting.Contracts;
using Shooting.Contracts.ShootingInfo;
using Shooting.Domain.Events.TargetLayerCreated;
using Shooting.Domain.Models;

namespace Shooting.Domain.Handlers.ShootingInfo;

public class CreateHitLayerMessageHandler : IMessageHandler<ShootingInfoContract>
{
    private readonly ILogger<CreateHitLayerMessageHandler> _logger;
    private readonly IMediator _mediator;

    public CreateHitLayerMessageHandler(ILogger<CreateHitLayerMessageHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IExecutionResult> HandleAsync(MessagingContext<ShootingInfoContract> messagingContext)
    {
        var payload = messagingContext.Message;
        await Task.Delay(TimeSpan.FromMilliseconds(Random.Shared.Next(200, 300)));
        _logger.LogInformation("Create Hit layer from {value} shots on Lane {laneNumber}", payload.Shots.Count, payload.LaneNumber);
        var targetLayer = new TargetLayer($"Hit layer {payload.LaneNumber}", 1);
        await _mediator.Publish(new TargetLayerCreatedEvent(payload.LaneNumber, targetLayer));
        return new SuccessfulResult();
    }
}