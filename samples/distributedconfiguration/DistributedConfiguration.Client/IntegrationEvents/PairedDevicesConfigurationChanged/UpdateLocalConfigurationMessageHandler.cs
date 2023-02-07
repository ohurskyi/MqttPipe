using DistributedConfiguration.Contracts.Pairing;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Results;

namespace DistributedConfiguration.Client.IntegrationEvents.PairedDevicesConfigurationChanged;

public class UpdateLocalConfigurationMessageHandler : IMessageHandler<DevicesConfigurationChangedContract>
{
    private readonly ILogger<UpdateLocalConfigurationMessageHandler> _logger;

    public UpdateLocalConfigurationMessageHandler(ILogger<UpdateLocalConfigurationMessageHandler> logger)
    {
        _logger = logger;
    }

    public async Task<IExecutionResult> HandleAsync(MessagingContext<DevicesConfigurationChangedContract> messagingContext)
    {
        var payload = messagingContext.Message;
        var newConfiguration = payload.PairedDevicesModel;
        _logger.LogInformation("New configuration received. Paired devices count: {value}. Updating local configuration... ", newConfiguration.Devices.Count);
        return await Task.FromResult(new SuccessfulResult());
    }
}