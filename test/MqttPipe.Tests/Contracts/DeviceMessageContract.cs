using MessagingLibrary.Core.Messages;

namespace MqttPipe.Tests.Contracts;

public class DeviceMessageContract : IMessageContract
{
    public string Name { get; set; }
}