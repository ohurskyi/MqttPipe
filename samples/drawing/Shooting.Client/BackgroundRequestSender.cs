using MessagingLibrary.Core.Clients;
using MqttPipe.Shooting;
using Shooting.Contracts.TargetLayers;
using Shooting.Contracts.Topics;

namespace Shooting.Client;

public class BackgroundRequestSender : BackgroundService
{
    private readonly IRequestClient<ShootingClientOptions> _requestClient;
    private readonly ILogger<BackgroundRequestSender> _logger;

    public BackgroundRequestSender(IRequestClient<ShootingClientOptions> requestClient, ILogger<BackgroundRequestSender> logger)
    {
        _requestClient = requestClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var laneNumber = Random.Shared.Next(1, 100);
                
            await SendRequest(laneNumber);
                
            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
        }
    }
        
    private async Task SendRequest(int laneNumber)
    {
        try
        {
            var targetLayers = await _requestClient.SendAndWaitAsync<GetTargetLayersRequest, GetTargetLayersResponse>(
                ShootingTopicConstants.ShootingInfoRequest,
                ShootingTopicConstants.ShootingInfoResponse,
                new GetTargetLayersRequest { LaneNumber = laneNumber },
                TimeSpan.FromSeconds(2));

            _logger.LogInformation("Received {layers} on lane {laneNumber}", targetLayers.Layers, laneNumber);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Error while sending sending {type} request.", typeof(GetTargetLayersRequest));
        }
    }
}