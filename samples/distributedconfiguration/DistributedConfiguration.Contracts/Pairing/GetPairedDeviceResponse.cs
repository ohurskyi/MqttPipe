using MessagingLibrary.Core.Messages;

namespace DistributedConfiguration.Contracts.Pairing;

public class GetPairedDeviceResponse : IMessageResponse
{
    public string DeviceId { get; set; }
        
    public string DeviceName { get; set; }
}