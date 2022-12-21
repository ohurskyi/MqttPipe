using MediatR;

namespace Shooting.Domain.Events.ShootingStartedEvent;

public class ShootingStartedEvent : INotification
{
    public ShootingStartedEvent(int lane)
    {
        Lane = lane;
    }

    public int Lane { get; }
}