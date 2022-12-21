using DistributedConfiguration.Contracts.Pairing;
using DistributedConfiguration.Contracts.Topics;
using MessagingLibrary.Core.Clients;
using MqttPipe.Infrastructure;

namespace DistributedConfiguration.Client;

public class BackgroundRequestSender : BackgroundService
{
    private int _requestCount = 0;
    private readonly IRequestClient<InfrastructureClientOptions> _requestClient;
    private readonly ILogger<BackgroundRequestSender> _logger;

    public BackgroundRequestSender(IRequestClient<InfrastructureClientOptions> requestClient, ILogger<BackgroundRequestSender> logger)
    {
        _requestClient = requestClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var deviceId = $"Device: {++_requestCount}";
                _logger.LogInformation("Try to get a device with id {value}", deviceId);
                var payload = new GetPairedDeviceContract { DeviceId = deviceId};
                var response = await _requestClient.SendAndWaitAsync<GetPairedDeviceContract, GetPairedDeviceResponse>(
                    DistributedConfigurationTopicConstants.RequestUpdate,
                    DistributedConfigurationTopicConstants.ResponseUpdate, payload,
                    TimeSpan.FromSeconds(2));
                _logger.LogInformation("Received response for device with id {value}", response?.DeviceId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error while sending sending {type} request.", typeof(GetPairedDeviceContract));
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}