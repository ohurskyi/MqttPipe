using MediatR;
using Shooting.Domain.Models;

namespace Shooting.Domain.Events.TargetLayerCreated;

public class TargetLayerCreatedEvent : INotification
{
    public TargetLayerCreatedEvent(int lane, TargetLayer targetLayer)
    {
        Lane = lane;
        TargetLayer = targetLayer;
    }

    public TargetLayer TargetLayer { get; }

    public int Lane { get; }
}