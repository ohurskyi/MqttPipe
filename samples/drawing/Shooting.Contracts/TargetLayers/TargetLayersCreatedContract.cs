using MessagingLibrary.Core.Messages;

namespace Shooting.Contracts.TargetLayers;

public class TargetLayersCreatedContract : IMessageContract
{
    public TargetLayersCreatedContract(List<string> layers)
    {
        Layers = layers;
    }

    public List<string> Layers { get; }
}