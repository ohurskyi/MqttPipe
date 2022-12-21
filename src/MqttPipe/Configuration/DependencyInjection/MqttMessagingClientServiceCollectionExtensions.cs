using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MqttPipe.Clients;
using MqttPipe.Configuration.Configuration;
using MqttPipe.Services;

namespace MqttPipe.Configuration.DependencyInjection
{
    public static class MqttMessagingClientServiceCollectionExtensions
    {
        public static IServiceCollection AddMqttMessagingClient<TMessagingClientOptions, TClientOptionsBuilder>(
            this IServiceCollection serviceCollection, 
            IConfiguration configuration)
            where TMessagingClientOptions : class, IMqttMessagingClientOptions, new()
            where TClientOptionsBuilder: class, IClientOptionsBuilder<TMessagingClientOptions>
        {
            serviceCollection.ConfigureMessagingClientOptions<TMessagingClientOptions>(configuration);

            serviceCollection.TryAddSingleton<IClientOptionsBuilder<TMessagingClientOptions>, TClientOptionsBuilder>();
            
            serviceCollection.TryAddSingleton<IMqttMessagingClient<TMessagingClientOptions>, MqttMessagingClient<TMessagingClientOptions>>();
            
            serviceCollection.AddMqttMessagingStartupServices<TMessagingClientOptions>();

            return serviceCollection;
        }

        private static IServiceCollection AddMqttMessagingStartupServices<TMessagingClientOptions>(this IServiceCollection serviceCollection)
            where TMessagingClientOptions : class, IMqttMessagingClientOptions, new()
        {
            serviceCollection.AddHostedService<MqttMessagingHostedService<TMessagingClientOptions>>();
            return serviceCollection;
        }
        
        private static IServiceCollection ConfigureMessagingClientOptions<TMessagingClientOptions>(this IServiceCollection serviceCollection, IConfiguration configuration) 
            where TMessagingClientOptions : class, IMqttMessagingClientOptions, new()
        {
            var sectionName = typeof(TMessagingClientOptions).Name;
            var configurationSection = configuration.GetSection(sectionName);
            serviceCollection.Configure<TMessagingClientOptions>(configurationSection);
            serviceCollection.AddSingleton(sp => sp.GetRequiredService<IOptions<TMessagingClientOptions>>().Value);

            return serviceCollection;
        }
    }
}