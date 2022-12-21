using MessagingLibrary.Core.Messages;

namespace Shooting.Contracts.TargetLayers;

public class GetTargetLayersResponse : IMessageResponse
{
    public List<string> Layers { get; set; }
}