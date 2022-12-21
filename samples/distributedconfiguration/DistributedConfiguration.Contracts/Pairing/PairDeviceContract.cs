using MessagingLibrary.Core.Messages;

namespace DistributedConfiguration.Contracts.Pairing;

public class PairDeviceContract : IMessageContract
{
    public string MacAddress { get; set; }
}