using MediatR;

namespace Shooting.Domain.IntegrationEvents.TargetLayersCreated;

public class TargetLayersCreatedIntegrationEvent : INotification
{
    public TargetLayersCreatedIntegrationEvent(List<string> layers)
    {
        Layers = layers;
    }

    public List<string> Layers { get; }
}