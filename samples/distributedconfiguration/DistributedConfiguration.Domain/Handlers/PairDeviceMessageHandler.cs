using DistributedConfiguration.Contracts.Models;
using DistributedConfiguration.Contracts.Pairing;
using DistributedConfiguration.Contracts.Topics;
using DistributedConfiguration.Domain.Infrastructure;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using Microsoft.Extensions.Logging;

namespace DistributedConfiguration.Domain.Handlers;

public class PairDeviceMessageHandler : MessageHandlerBase<PairDeviceContract>
{
    private readonly ILogger<PairDeviceMessageHandler> _logger;

    private readonly DevicesStorage _devicesStorage;

    public PairDeviceMessageHandler(ILogger<PairDeviceMessageHandler> logger, DevicesStorage devicesStorage)
    {
        _logger = logger;
        _devicesStorage = devicesStorage;
    }

    protected override async Task<IExecutionResult> HandleAsync(MessagingContext<PairDeviceContract> messagingContext)
    {
        var payload = messagingContext.Payload;
        
        if (_devicesStorage.Exists(payload.MacAddress))
        {
            return new SuccessfulResult();
        }
        
        _logger.LogInformation("Paired with device {value}", payload.MacAddress);

        _devicesStorage.Add(new Device { MacAddress = payload.MacAddress });

        var eventPayload = new DevicesConfigurationChangedContract
        {
            PairedDevicesModel = new PairedDevicesModel { Devices = _devicesStorage.GetAll() }
        };
        
        var integrationEventResult = new IntegrationEventResult(eventPayload, DistributedConfigurationTopicConstants.CurrentConfiguration);

        return await Task.FromResult(integrationEventResult);
    }
}