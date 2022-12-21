using MediatR;
using Microsoft.Extensions.Logging;
using Shooting.Domain.Infrastructure;

namespace Shooting.Domain.Events.ShootingStartedEvent;

public class IncrementActivateLanesCountEventHandler : INotificationHandler<ShootingStartedEvent>
{
    private readonly ILogger<IncrementActivateLanesCountEventHandler> _logger;
    private readonly ActiveShootingLanesStorage _activeShootingLanesStorage;

    public IncrementActivateLanesCountEventHandler(ILogger<IncrementActivateLanesCountEventHandler> logger, ActiveShootingLanesStorage activeShootingLanesStorage)
    {
        _logger = logger;
        _activeShootingLanesStorage = activeShootingLanesStorage;
    }

    public Task Handle(ShootingStartedEvent notification, CancellationToken cancellationToken)
    {
        var currentActiveLanes = _activeShootingLanesStorage.IncrementActivateLanesCount(notification.Lane);
        _logger.LogInformation("Current active lanes count {value}", currentActiveLanes);
        return Task.CompletedTask;
    }
}