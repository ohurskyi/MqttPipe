using MessagingLibrary.Core.Messages;

namespace Shooting.Contracts.TargetLayers;

public class GetTargetLayersRequest : IMessageRequest
{
    public int LaneNumber { get; set; }
}