using MessagingLibrary.Core.Factory;
using MessagingLibrary.Core.Handlers;
using MessagingLibrary.Core.Messages;
using MessagingLibrary.Core.Results;
using Microsoft.Extensions.Logging;
using Shooting.Contracts.TargetLayers;
using Shooting.Domain.Infrastructure;

namespace Shooting.Domain.Handlers.GetTargetLayers;

public class GetTargetLayersMessageHandler : MessageHandlerNew<GetTargetLayersRequest>
{
    private readonly TargetLayersStorage _targetLayersStorage;
    private readonly ILogger<GetTargetLayersMessageHandler> _logger;

    public GetTargetLayersMessageHandler(TargetLayersStorage targetLayersStorage, ILogger<GetTargetLayersMessageHandler> logger)
    {
        _targetLayersStorage = targetLayersStorage;
        _logger = logger;
    }

    protected override async Task<IExecutionResult> HandleAsync(MessagingContextNew<GetTargetLayersRequest> messagingContext)
    {
        var payload = messagingContext.Message;
        var laneNumber = payload.LaneNumber;
        _logger.LogInformation("Getting target layers on lane {value}", laneNumber);
        var layers = _targetLayersStorage.GetTargetLayers(laneNumber);
        var response = new GetTargetLayersResponse { Layers = layers.Select(s => s.Layer).ToList() };
        var result = new ReplyResult(response, messagingContext);
        return await Task.FromResult(result);
    }
}