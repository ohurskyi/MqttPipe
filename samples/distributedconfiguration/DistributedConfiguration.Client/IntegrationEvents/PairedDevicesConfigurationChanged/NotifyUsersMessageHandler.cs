using DistributedConfiguration.Contracts.Pairing;
using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Results;

namespace DistributedConfiguration.Client.IntegrationEvents.PairedDevicesConfigurationChanged;

public class NotifyUsersMessageHandler : IMessageHandler<DevicesConfigurationChangedContract>
{
    private readonly ILogger<NotifyUsersMessageHandler> _logger;

    public NotifyUsersMessageHandler(ILogger<NotifyUsersMessageHandler> logger)
    {
        _logger = logger;
    }

    public async Task<IExecutionResult> HandleAsync(MessagingContext<DevicesConfigurationChangedContract> messagingContext)
    {
        _logger.LogInformation("Notify users about distributed config change");
        return await Task.FromResult(new SuccessfulResult());
    }
}