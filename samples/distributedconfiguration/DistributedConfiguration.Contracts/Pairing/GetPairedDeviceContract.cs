using MessagingLibrary.Core.Messages;

namespace DistributedConfiguration.Contracts.Pairing;

public class GetPairedDeviceContract : IMessageRequest
{
    public string DeviceId { get; set; }
}