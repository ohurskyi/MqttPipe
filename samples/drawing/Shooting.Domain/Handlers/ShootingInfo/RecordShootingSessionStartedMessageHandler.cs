using MediatR;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using Microsoft.Extensions.Logging;
using Shooting.Contracts.ShootingInfo;
using Shooting.Domain.Events;
using Shooting.Domain.Events.ShootingStartedEvent;

namespace Shooting.Domain.Handlers.ShootingInfo;

public class RecordShootingSessionStartedMessageHandler :  MessageHandlerBase<ShootingInfoContract> 
{
    private readonly ILogger<RecordShootingSessionStartedMessageHandler> _logger;
    private readonly IMediator _mediator;

    public RecordShootingSessionStartedMessageHandler(IMediator mediator, ILogger<RecordShootingSessionStartedMessageHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    protected override async Task<IExecutionResult> HandleAsync(MessagingContext<ShootingInfoContract> messagingContext)
    {
        var payload = messagingContext.Payload;
        _logger.LogInformation("Shooting started on Lane {laneNumber}",  payload.LaneNumber);
        await _mediator.Publish(new ShootingStartedEvent(payload.LaneNumber));
        return new SuccessfulResult();
    }
}