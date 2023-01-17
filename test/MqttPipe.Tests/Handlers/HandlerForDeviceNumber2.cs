using System.IO;
using System.Threading.Tasks;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MqttPipe.Tests.Contracts;

namespace MqttPipe.Tests.Handlers;

public class HandlerForDeviceNumber2 :  MessageHandlerNew<DeviceMessageContract>
{
    private readonly TextWriter _textWriter;

    public HandlerForDeviceNumber2(TextWriter textWriter)
    {
        _textWriter = textWriter;
    }

    protected override async Task<IExecutionResult> HandleAsync(MessagingContextNew<DeviceMessageContract> messagingContext)
    {
        var payload = messagingContext.Message;
        await _textWriter.WriteLineAsync(payload.Name + " " + nameof(HandlerForDeviceNumber2));
        return new SuccessfulResult();
    }
}