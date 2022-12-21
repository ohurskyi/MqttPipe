using DistributedConfiguration.Contracts.Models;
using MessagingLibrary.Core.Messages;

namespace DistributedConfiguration.Contracts.Pairing;

public class DevicesConfigurationChangedContract : IMessageContract
{
    public PairedDevicesModel PairedDevicesModel { get; set; }
}