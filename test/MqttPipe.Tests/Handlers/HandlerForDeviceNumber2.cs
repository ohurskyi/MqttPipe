using System.IO;
using System.Threading.Tasks;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Results;
using MqttPipe.Tests.Contracts;

namespace MqttPipe.Tests.Handlers;

public class HandlerForDeviceNumber2 :  IMessageHandler<DeviceMessageContract>
{
    private readonly TextWriter _textWriter;

    public HandlerForDeviceNumber2(TextWriter textWriter)
    {
        _textWriter = textWriter;
    }

    public async Task<IExecutionResult> HandleAsync(MessagingContext<DeviceMessageContract> messagingContext)
    {
        var payload = messagingContext.Message;
        await _textWriter.WriteLineAsync(payload.Name + " " + nameof(HandlerForDeviceNumber2));
        return new SuccessfulResult();
    }
}