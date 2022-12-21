using MediatR;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using Microsoft.Extensions.Logging;
using Shooting.Contracts.ShootingInfo;
using Shooting.Domain.Events.TargetLayerCreated;
using Shooting.Domain.Models;

namespace Shooting.Domain.Handlers.ShootingInfo;

public class CreateTargetLayerMessageHandler : MessageHandlerBase<ShootingInfoContract>
{
    private readonly ILogger<CreateTargetLayerMessageHandler> _logger;
    private readonly IMediator _mediator;

    public CreateTargetLayerMessageHandler(ILogger<CreateTargetLayerMessageHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    protected override async Task<IExecutionResult> HandleAsync(MessagingContext<ShootingInfoContract> messagingContext)
    {
        var payload = messagingContext.Payload;
        await Task.Delay(TimeSpan.FromMilliseconds(Random.Shared.Next(1000, 2000)));
        _logger.LogInformation("Create Target layer from {value} shots on Lane {laneNumber}", payload.Shots.Count, payload.LaneNumber);
        var targetLayer = new TargetLayer($"Target layer {payload.LaneNumber}", 0);
        await _mediator.Publish(new TargetLayerCreatedEvent(payload.LaneNumber, targetLayer));
        return await Task.FromResult(new SuccessfulResult());
    }
}