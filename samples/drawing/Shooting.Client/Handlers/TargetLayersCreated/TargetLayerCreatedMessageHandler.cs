using MessagingLibrary.Core.Contexts;
using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using Shooting.Contracts.TargetLayers;

namespace Shooting.Client.Handlers.TargetLayersCreated;

public class TargetLayerCreatedMessageHandler : IMessageHandler<TargetLayersCreatedContract>
{
    private readonly ILogger<TargetLayerCreatedMessageHandler> _logger;

    public TargetLayerCreatedMessageHandler(ILogger<TargetLayerCreatedMessageHandler> logger)
    {
        _logger = logger;
    }

    public async Task<IExecutionResult> HandleAsync(MessagingContext<TargetLayersCreatedContract> messagingContext)
    {
        var layers = messagingContext.Message.Layers;
        _logger.LogInformation("Send Created {layer} to UI", layers);
        await Task.Delay(TimeSpan.FromMilliseconds(1200));
        return new SuccessfulResult();
    }
}