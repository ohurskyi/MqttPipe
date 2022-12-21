using DistributedConfiguration.Contracts.Pairing;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;

namespace DistributedConfiguration.Client.IntegrationEvents.PairedDevicesConfigurationChanged;

public class UpdateLocalConfigurationMessageHandler : MessageHandlerBase<DevicesConfigurationChangedContract>
{
    private readonly ILogger<UpdateLocalConfigurationMessageHandler> _logger;

    public UpdateLocalConfigurationMessageHandler(ILogger<UpdateLocalConfigurationMessageHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<IExecutionResult> HandleAsync(MessagingContext<DevicesConfigurationChangedContract> messagingContext)
    {
        var payload = messagingContext.Payload;
        var newConfiguration = payload.PairedDevicesModel;
        _logger.LogInformation("New configuration received. Paired devices count: {value}. Updating local configuration... ", newConfiguration.Devices.Count);
        return await Task.FromResult(new SuccessfulResult());
    }
}