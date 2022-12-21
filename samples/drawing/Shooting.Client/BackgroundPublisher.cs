using MessagingLibrary.Core.Clients;
using MqttPipe.Shooting;
using Shooting.Contracts.ShootingInfo;
using Shooting.Contracts.Topics;

namespace Shooting.Client
{
    public class BackgroundPublisher : BackgroundService
    {
        private readonly IMessageBus<ShootingClientOptions> _messageBus;

        private readonly ILogger<BackgroundPublisher> _logger;

        public BackgroundPublisher(IMessageBus<ShootingClientOptions> messageBus, IRequestClient<ShootingClientOptions> requestClient, ILogger<BackgroundPublisher> logger)
        {
            _messageBus = messageBus;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var laneNumber = Random.Shared.Next(1, 100);
                
                var message = new ShootingInfoContract { Shots = Enumerable.Range(1, 10).Select(s => new Shot()).ToList(), LaneNumber = laneNumber };

                await _messageBus.Publish(message, ShootingTopicConstants.ShootingInfoTopic);
                
                _logger.LogInformation("Published {message} into queue on lane {laneNumber}", nameof(ShootingInfoContract), laneNumber);

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}