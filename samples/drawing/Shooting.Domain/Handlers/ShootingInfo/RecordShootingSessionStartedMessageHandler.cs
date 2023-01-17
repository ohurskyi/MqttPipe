using MediatR;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using Microsoft.Extensions.Logging;
using Shooting.Contracts.ShootingInfo;
using Shooting.Domain.Events;
using Shooting.Domain.Events.ShootingStartedEvent;

namespace Shooting.Domain.Handlers.ShootingInfo;

public class RecordShootingSessionStartedMessageHandler :  MessageHandlerNew<ShootingInfoContract> 
{
    private readonly ILogger<RecordShootingSessionStartedMessageHandler> _logger;
    private readonly IMediator _mediator;

    public RecordShootingSessionStartedMessageHandler(IMediator mediator, ILogger<RecordShootingSessionStartedMessageHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    protected override async Task<IExecutionResult> HandleAsync(MessagingContextNew<ShootingInfoContract> messagingContext)
    {
        var payload = messagingContext.Message;
        _logger.LogInformation("Shooting started on Lane {laneNumber}",  payload.LaneNumber);
        await _mediator.Publish(new ShootingStartedEvent(payload.LaneNumber));
        return new SuccessfulResult();
    }
}