using System.IO;
using System.Threading.Tasks;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Results;
using MqttPipe.Tests.Contracts;

namespace MqttPipe.Tests.Handlers;

public class HandlerForAllDeviceNumbers : IMessageHandler<DeviceMessageContract>
{
    private readonly TextWriter _textWriter;

    public HandlerForAllDeviceNumbers(TextWriter textWriter)
    {
        _textWriter = textWriter;
    }

    public async Task<IExecutionResult> HandleAsync(MessagingContext<DeviceMessageContract> messagingContext)
    {
        var payload = messagingContext.Message;
        await _textWriter.WriteLineAsync(payload.Name + " " + nameof(HandlerForAllDeviceNumbers));
        return new SuccessfulResult();
    }
}