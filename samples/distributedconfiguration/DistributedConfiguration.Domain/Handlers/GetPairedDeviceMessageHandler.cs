using DistributedConfiguration.Contracts.Pairing;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Results;
using Microsoft.Extensions.Logging;

namespace DistributedConfiguration.Domain.Handlers;

public class GetPairedDeviceMessageHandler : IMessageHandler<GetPairedDeviceContract>
{
    private readonly ILogger<PairDeviceMessageHandler> _logger;

    public GetPairedDeviceMessageHandler(ILogger<PairDeviceMessageHandler> logger)
    {
        _logger = logger;
    }

    public async Task<IExecutionResult> HandleAsync(MessagingContext<GetPairedDeviceContract> messagingContext)
    {
        var payload = messagingContext.Message;
        
        _logger.LogInformation("Get device with id {value}", payload.DeviceId);

        var response = new GetPairedDeviceResponse {
            DeviceId = payload.DeviceId, 
            DeviceName = $"{payload.DeviceId}-{Guid.NewGuid()}-D",
        };

        var result = new ReplyResult(response, messagingContext);
        
        return await Task.FromResult(result);
    }
}