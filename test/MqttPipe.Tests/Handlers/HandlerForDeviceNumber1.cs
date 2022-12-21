using System.IO;
using System.Threading.Tasks;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using MqttPipe.Tests.Contracts;

namespace MqttPipe.Tests.Handlers;

public class HandlerForDeviceNumber1 : MessageHandlerBase<DeviceMessageContract>
{
    private readonly TextWriter _textWriter;

    public HandlerForDeviceNumber1(TextWriter textWriter)
    {
        _textWriter = textWriter;
    }
    
    protected override async Task<IExecutionResult> HandleAsync(MessagingContext<DeviceMessageContract> messagingContext)
    {
        var payload = messagingContext.Payload;
        await _textWriter.WriteLineAsync(payload.Name + " " + nameof(HandlerForDeviceNumber1));
        return new SuccessfulResult();
    }
}